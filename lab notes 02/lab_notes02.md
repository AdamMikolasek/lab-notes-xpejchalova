# Lab Notes - Assignment 2

#Lab 2 - Multicolumn Indices, Joins & Aggregations
> Lenka Pejchalova

## 1. See how  `like`  with a leading pattern is slow

> SELECT * 
> FROM documents 
> WHERE supplier_ico 
> LIKE '%5733'

Runtime: ~ 200 msec

> CREATE INDEX index_supplier_ico_text_pattern_ops 
> ON documents(supplier_ico text_pattern_ops);

Runtime: ~ 185 msec

Both cases use Seq Scan. It's slow because the search pattern is at the end of the expression, so in both cases (with and without index) all values must be searched.


## 2. Try  `like`  with a trailing pattern

Without index
> EXPLAIN ANALYZE SELECT * 
> FROM documents 
> WHERE supplier_ico 
> LIKE '57%';`

Runtime: ~ 180 msec

With index
> CREATE INDEX index_supplier_ico_text_pattern_ops 
> ON documents(supplier_ico text_pattern_ops);

> EXPLAIN ANALYZE SELECT * 
> FROM documents 
> WHERE supplier_ico 
> LIKE '57%';`

Runtime: ~ 111 msec

The condition in _Index Cond_ means that all values in the index are sorted alphabetically and according to the value, and we are searching for those that begin with the prefix 57.


## 3. Make the suffix search fast

>CREATE INDEX index_supplier_ico_text_pattern_ops 
>ON documents(reverse(supplier_ico) text_pattern_ops);

>SELECT * 
>FROM documents 
>WHERE reverse(supplier_ico) 
>LIKE reverse('%5733')

Search plan: Bitmap Heap Scan 
Runtime: 56 msec

The reverse function speeds up search through the suffix. The problem - slow suffix searching - from the previous examples, can be solved this way.

## 5. Understand covering index

Without index:
> SELECT customer 
> FROM documents 
> WHERE department = 'Rozhlas a televizia Slovenska'

Search plan: Seq Scan 
Runtime: 191 msec

With index:
> CREATE INDEX index_on_documents 
> ON documents(department);

>SELECT customer 
>FROM documents 
>WHERE department = 'Rozhlas a televizia Slovenska'

Search plan: Bitmap Index Scan 
Runtime: 151 msec

Covering index: 
>CREATE INDEX index_on_documents
ON documents(department,customer);

>SELECT customer 
>FROM documents 
>WHERE department = 'Rozhlas a televizia Slovenska'

Search plan: Bitmap Index Scan 
Runtime: 100 msec

Covering index speeds up filtering more than ordinary index. 
