# Outlou

The application (as was required) uses the Sqlite as the database. In order to read an RSS feed I used XmlReader and SyndicationFeed, I also added AutoMapper (Dependency Injection version) in order to map the SyndicationItem to my custom FeedItem object. I also added logger to log the errors and information.

In order to use the application you should first register a user using api/account/register (the password must be 4-8 lengh with at least one upper case letter, one lower case letter, one symbol, and one number ex.Pa$$w0rd), then you can use api/account/login in order to get a token (you are also receiving it back after successfull registration). Then, if you use Swagger, I set up the Authorize button in order to pass the token and authorize all further actions, or if You use postman You can pass the token as Authorization header with value "Bearer your_token". 
