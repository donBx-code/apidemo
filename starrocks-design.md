

# Part 2 – StarRocks / MPP Design Challenge

**Estimated Time:** 30–40 minutes

### Objective

This design exercise evaluates your understanding of **MPP analytical databases**, **data lakehouse ingestion**, and **query performance at scale**.

You are not expected to have hands-on StarRocks experience. We are evaluating **how you think**, not product-specific syntax.

Submit your answers as:

```
part2-starrocks/starrocks-design.md
```

---

## Scenario

You are designing a data pipeline and query layer for a lakehouse-backed analytics platform.

* Source systems produce **daily data files**
* Data is landed in **S3-compatible object storage**
* Data is queried using an **MPP analytical database** such as StarRocks
* APIs sit on top of the analytical database to serve downstream applications

### Dataset Characteristics

* ~500 million rows per year
* Append-only (no updates or deletes)
* Columns:

  * `client_id`
  * `fund_id` (nullable)
  * `as_of_date`
  * `metric_name`
  * `metric_value`

### Query Characteristics

* Almost all queries filter by `client_id`
* Many queries also filter by `fund_id`
* Almost all queries filter by a date range
* Common queries aggregate data (SUM, COUNT, AVG)

---

## Questions

### 1. Object Storage Layout

How would you organize this data in object storage?

Consider:

* Directory / prefix structure  - the directory structuer will based on partioning strategy so you can easily manage you data based on teh cut of data in this case the AS_OF_DATE
* Partitioning strategy -> In consideration of the number for records partition can be done by as of date and sub partion on client  and fund_id

* File formats parquet format is a cadidata due columnar storate, compression , filters are applied at the storage , it also support schema evolution
* Handling late or reprocessed data  - we can implement a version attribute such that we can filter it based on latest data

---

### 2. MPP Table Design

How would you design the analytical table(s)?

Consider:

* Partition keys  -
        In this case  Use AS_OF_DATE, CLIENT_ID and FUND_ID; use logical grouping of data.
* Distribution / bucketing strategy - 
        In addtion to identified partition in troduce randon partions (salting) if data grows. 
* Sort or clustering choices
        Implement  replication, hash distribution, round robin distribution across the cluster so data are avalable to in the nodes.  
* Duplicate vs primary key-style designs

---

### 3. API & Query Design

How should APIs be designed to work efficiently with an MPP system?

Consider:

* Safe vs unsafe query patterns - 
            Safe queries involed filter on primary partions defined.   Dont use bulk queries data with limited filtering condition; this  will overload your DB and API infrustructure. 
* Pagination strategies
             Use offset and limits to queries to enable limited data scanning. 
* Avoiding full table scans
             In addition to pagination ; provide default paging  and limit
* Handling large aggregations
             For larger aggegation create a materialized view  that can be updated on a regualr basis.  This will remove burden to the DB of frequent aggregate request. 

---

### 4. Failure Modes & Operations

What failure modes do you expect, and how would you mitigate them?

Examples:

* Data skew
        Revisit you data model and look at repartioning. 
        Look at queries and procedures to optimize joins
        Implement partion salting  so you can optimize queries 
        
* Partition explosion
        Similar to data skey , impelement repartioning, by identifying other cadidate keys
        Look at partion salting if not other kcandicate keys are availalble
* Small file problems
        Revist the partitioning strategy (drop) to  that data are grouped and samll slices of data are prevented. 
* Query latency regression
        Introduce granular queries parameters so that data that are scanned are 
* Cost growth
        Implemtn a rolling house keeping.  So that you can effecinetly manage data grown, Separate WARM and HOT datasets.   This can be done in this case a rolling mantenance on AS_OFF_DATE. 

---

### 5. Scale Thought Experiment

Assume data volume grows **10× over two years**.

* What breaks first?
   Assuming the object storage is maintained properly and housekeeping are working, It is ussually the API's for  delivery will  break given they were not designed to handle the amunt of data.


* What would you change?
    The API infrustructure should be able to scale based on the need.   Dynamic allocation/horizontal of services scaling are needed.
    Further more message brokers can be looked at so that clients can subscribe to a publish comsume model.  
     

---

## Evaluation Criteria

Across both parts, we are evaluating:

* Backend API fundamentals
* Data modeling instincts
* MPP vs OLTP understanding
* Trade-off awareness
* Clarity of communication

There is no single correct answer. Explain your reasoning.

---

## Final Notes

* Focus on clarity over perfection
* State assumptions explicitly
* Partial solutions are acceptable
* This assessment is designed to be completed within **~2 hours**
