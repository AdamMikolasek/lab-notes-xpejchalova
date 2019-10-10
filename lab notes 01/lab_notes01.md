# Lab Notes - Assignment 1

Lab Notes 1 - Query plans & indexing
> Lenka Pejchalova

## **Try how indexes make a query faster**

- **Write a simple query filtering on a single column (e.g. `supplier = ''`, or `department = ''`). Measure how long the query takes.**
	> EXPLAIN ANALYZE SELECT * 
	FROM documents 
	WHERE supplier = 'Orange Slovensko, a.s.';
	
Runtime: ~ 99.366 ms
	

- **Add an index for the column and measure how fast is the query now. What plan did the database use before & after index?**
	> CREATE INDEX index_documents_on_supplier 
	ON documents(supplier);
	
	> EXPLAIN ANALYZE SELECT * 
	FROM documents 
	WHERE supplier = 'Orange Slovensko, a.s.';


**Before index**: Seq Scan
	Runtime: ~ 99.366 ms

**After index**: Bitmap Heap Scan
	Runtime: ~ 0.028 ms


- **Write a simple range query on a single column (e.g., `total_amount > 100000 and total_amount 999999999`). Measure how long the query takes.**
		
	> EXPLAIN ANALYZE SELECT * 
	   FROM documents 
	   WHERE total_amount > 2222 AND total_amount > 1333;

Runtime: ~ 111.383 ms


- **Add an index for the column and measure how fast is the query now. What plan did the database use before & after index?** 

	> CREATE INDEX index_documents_on_total_amount 
	ON documents(total_amount);
	
	> EXPLAIN ANALYZE SELECT * 
	FROM documents 
	WHERE total_amount > 2222 AND total_amount > 1333;

**Before index**: Seq Scan
	Runtime: ~ 111.383 ms

**After index**: Bitmap Heap Scan
	Runtime: ~ 13.364 ms

**Conclusion:** Indexes accelerate query process. Creating indexes on columns makes queries quicker.

## **Try how indexes slow down writes**
- **Drop all indexes on the `documents` table**

> DROP INDEX index_documents_on_supplier, index_documents_on_total_amount;

-   **Benchmark how long does it take to insert a batch of N rows into the `documents` table.**
> EXPLAIN ANALYZE INSERT INTO documents 
> (SELECT * FROM documents LIMIT 1000);

Runtime: ~ 13.364 ms
	
-   **Create index on a single column in the `documents` table. Choose an arbitrary column.**

> CREATE INDEX total_amount_index 
> ON documents (total_amount);

Runtime: ~ 22.4 ms

-   **Repeat the benchmark with 2 indices and 3 indices.**

> CREATE INDEX department_index 
> ON documents (department);

Runtime: ~ 27.6 ms

> CREATE INDEX supplier_index 
> ON documents (supplier);

Runtime: ~ 77.9 ms

-  **Now drop all indices and try if there's a difference in insert performance when you have a single index over low cardinality column vs. high cardinality column.**

Low cardinality column  -  _department_ - 84 distinct values  
> SELECT count (distinct department) 
> FROM documents;

>  INSERT INTO documents 
> (SELECT * FROM documents LIMIT 1000);

Runtime:  ~ 4.5 ms

> INSERT INTO documents 
> (SELECT * FROM documents LIMIT 10000);

Runtime: ~ 44 ms

High cardinality column  - _supplier_ - 138 789 distinct values
> SELECT count (distinct supplier) 
> FROM documents;

> INSERT INTO documents 
> (SELECT * FROM documents LIMIT 10000);

Runtime: ~ 23 ms


**Conclusion:** Indexes slows down the insertion process. High cardinality columns are slower than low cardinality columns.
