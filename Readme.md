
How to run 

1. go to the apidemo directory  and  build docker image 
2. run docker compose up -d

Test URLS to use :  


Query, Offsets and Pagination: 

/clients
/clients?offset=0&limit=20
/clients?offset=100&limit=50

/clients/101 - all records for client 101
/clients/101?fund_id=1001 - client 101, fund 1001 only
/clients/101?start_date=2025-01-01&end_date=2025-06-30 - client 101 within date range
/clients/101?fund_id=1001&start_date=2025-01-01&end_date=2025-06-30 - all filters combined

Aggregartion: 

/summary/100
/summary/100?fund_id=1000
summary/100?start_date=2025-01-01&end_date=2025-06-30



Swagger URL : http://localhost:5001/swagger/index.html