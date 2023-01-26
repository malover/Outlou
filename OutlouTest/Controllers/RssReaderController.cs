using System.Collections.Concurrent;
using System.Globalization;
using System.ServiceModel.Syndication;
using System.Xml;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutlouTest.Data;
using OutlouTest.Models;

namespace OutlouTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RssReaderController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public RssReaderController(DataContext context, ILogger<RssReaderController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddRssFeed(FeedSource feedSource)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Error occured when provided model didn't pass the validation.");
                return BadRequest();
            }

            var feed = await _context.FeedSources.FirstOrDefaultAsync(x => x.Url == feedSource.Url);
            var validFeed = IsValidFeedUrl(feedSource.Url);

            if (feed != null)
            {
                _logger.LogError("Error occured due to the duplicate source.");
                return BadRequest("This feed source is already presented.");
            }
            if (!validFeed)
            {
                _logger.LogError("Error occured validating the provided url.");
                return BadRequest("Invalid feed url format.");
            }

            _context.FeedSources.Add(feedSource);
            _context.SaveChanges();

            _logger.LogInformation("RSS source added successfully.");
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> LoadFeed()
        {
            var rssSources = await _context.FeedSources.ToListAsync();

            if (!rssSources.Any())
            {
                _logger.LogError("Error occured due to the fact, that no sources were found.");
                return BadRequest("No sources found, try adding rss source using Post method first.");
            }

            var newItems = new ConcurrentBag<FeedItem>();

            Parallel.ForEach(rssSources, async source =>
            {
                using (XmlReader reader = XmlReader.Create(source.Url))
                {
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    if (feed.Items.Any())
                    {
                        var existingIds = _context.FeedItems.Select(x => x.Id).ToList();
                        var newFeedItems = feed.Items.Where(x => !existingIds.Contains(x.Id))
                            .Select(x => _mapper.Map<FeedItem>(x))
                            .ToList();
                        newFeedItems.ForEach(x =>
                        {
                            if (x.Summary == null) x.Summary = x.Title;
                        });
                        _context.FeedItems.AddRange(newFeedItems);
                        await _context.SaveChangesAsync();
                        newFeedItems.ForEach(x => newItems.Add(x));
                    }
                }
            });

            _logger.LogInformation("Feed successfully loaded and sent back with a response.");
            return Ok(newItems);
        }

        [HttpGet("yyyy-MM-dd")]
        public async Task<IActionResult> GetUnreadNewsByDate(string date)
        {
            if (!DateIsValid(date))
            {
                _logger.LogError($"Error occured validating the provided date string: {date}.");
                return BadRequest("The date format is invalid, please check the valid input of date parameter.");
            }

            var feed = await _context.FeedItems.Where(x => x.IsRead == false && x.PublishDate.Date == DateTime.Parse(date).Date).ToListAsync();

            if (feed is null)
            {
                _logger.LogError($"Error occured due to the fact that there is no unread news for the date: {date}");
                return BadRequest($"There is no unread feed for {date} date.");
            }

            _logger.LogInformation($"Unread feed for the {date} date was found and returned with a response.");
            return Ok(feed);
        }

        [HttpPut("id")]
        public async Task<IActionResult> SetNewsAsRead(string id)
        {
            var news = await _context.FeedItems.FindAsync(id);

            if (news is null)
            {
                _logger.LogError($"Error occured looking for a news with id: {id}, no news with such id");
                return BadRequest($"There is no news with {id} id, please choose the valid id and try again");
            }

            news.IsRead = true;

            _context.Entry(news).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Setting the {id} id news status to isRead = true was successfull.");
            return NoContent();
        }

        private bool DateIsValid(string date)
        {
            bool isValid = false;

            var dateFormats = "yyyy-MM-dd";

            DateTime scheduleDate;
            isValid = DateTime.TryParseExact(
                date,
                dateFormats,
                DateTimeFormatInfo.InvariantInfo,
                DateTimeStyles.None,
                        out scheduleDate);

            return isValid;
        }
        private bool IsValidFeedUrl(string url)
        {
            bool isValid = true;
            try
            {
                XmlReader reader = XmlReader.Create(url);
                Rss20FeedFormatter formatter = new Rss20FeedFormatter();
                formatter.ReadFrom(reader);
                reader.Close();
            }
            catch
            {
                isValid = false;
            }

            return isValid;
        }
    }
}

