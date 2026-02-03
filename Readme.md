
How to run 

1. go to the apidemo directory  and  build docker image 
        dotnet build -t apidemo .

2. run docker compose up -d
3: Open Swagger : http://localhost:5001/swagger/index.html


Actual 
Test URLS to use :  


Query, Offsets and Pagination: 

http://localhost:5001/clients  -> return 1st 10 records 
http://localhost:5001/clients?offset=0&limit=20      -> return 1st 20 records 
http://localhost:5001/clients?offset=100&limit=50      -> return  50 records of offset 100 

http://localhost:5001/clients/101 -  client 101
http://localhost:5001/clients/101?fund_id=1001 - client 101, fund 1001 only
http://localhost:5001/clients/101?start_date=2025-01-01&end_date=2025-06-30 - client 101 within date range
http://localhost:5001/clients/101?fund_id=1001&start_date=2025-01-01&end_date=2025-06-30 - all filters combined

Aggregartion: 

http://localhost:5001/summary/100.    -> aggregation for client 100
http://localhost:5001/summary/100?fund_id=1000 -> ggregation for fund  1000
http://localhost:5001/summary/100?start_date=2025-01-01&end_date=2025-06-30 -> aggregation for client 100 based on date range



