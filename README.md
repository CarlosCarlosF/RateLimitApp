# RateLimitApp

RateLimitApplication is a simple web application which will serve HTTP requests and support rate limiting on them.

## Introduction

 ### 1. Technologies used 

 - C# programming language 
 - Redis database
 - ServiceStack framework 

 ### 2. Application functionality

Application is exposing a route that is responding to 'GET' requests.

>  /greet/{Name}

In the response the client is getting back a simple message  `Hi,{Name}` and status `200 OK` if everything is going by plan.  `{Name}` is the route parameter. 
The endpoint is rate limited by originating the IP address and doesn't allow more then 10 requests per minute. 
When the limit is reached, the application is responding with a message `Rate limit exceded` and response status [`429 Too many requests`](https://www.webfx.com/web-development/glossary/http-status-codes/what-is-a-429-status-code/).

On every response the application calculates how many requests are still remaining and what is the end limit. 
The information is present on every response in Header section with present header keys

 - X-RateLimit-Limit
 - X-RateLimit-Remaining

## Application flow

```
Application gets a request from the customer with parameter {Name}
Aplication stores every request in the db with folowing parameter:
< IPAdress of the client
< Counter -represents the number of counts of the request
< StartTime -represents the start time of the request
< AttempTime - represents the time of request attemp
<LimitTime - represents the limit time 

Application has a mechanism of checking time of attemps to not brake its functionality.
Its forbidden to make more then 10 attemps in 1 minut. 
If the attemp count is less then 10 attemps in 1 minut the client should receive status 200 OK.
If the attemp count is more then 10 attemps in 1 minut the client shoudl receive status 429 TOOMANYREQUESTS.  
On every response the application generates information for remaining attemps and limit of attemps. 
```

## Testing 

 - Run application 
 - Open postmen
 - Make GET request with parameter {Name}
 - Try to make one attempt 
 - Try to make more then 10 attempts in one minute
 - Try to make several attempts less then 10, then wait for (one, two minutes) and make one more attempt the counter should be reset to one attempt
