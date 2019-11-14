# Lab Notes - Assignment 6


Lab 6 - Recursive
> Lenka Pejchalova

**1. Write a recursive query which returns a number and its factorial for all numbers up to 10.** :coffee: 

>WITH RECURSIVE fact(n, fact) AS (  
>SELECT 0, 1  
>UNION ALL  
>SELECT f.n+1, f.fact * (f.n+1) 
>FROM fact f )  
>SELECT *
>FROM fact limit 11;
	
**Results:**

| number| factorial|
|--|--|
| 0| 1|
| 1| 1|
| 2| 2|
| 3| 6|
| 4| 24|
| 5| 120|
| 6| 720|
| 7| 5040|
| 8| 40320|
| 9| 362880|
| 10| 3628800|

**2. Write a recursive query which returns a number and the number in Fibonacci sequence at that position for the first 20 Fibonacci numbers.** :coffee::coffee:

>WITH RECURSIVE fib(n, fib1, fib2) AS (  
>SELECT 1, 1, 1  
>UNION ALL
>SELECT n+1, fib2, fib1 + fib2 
> FROM fib f )  
> SELECT n, fib1 
> FROM fib limit 20;
	
**Results:**

|number|fibonacci|
|---|---|
|0|0|
|1|1|
|2|1|
|3|2|
|4|3|
|5|5|
|6|8|
|7|13|
|8|21|
|9|34|
|10|55|
|11|89|
|12|144|
|13|233|
|14|377|
|15|610|
|16|987|
|17|1597|
|18|2584|
|19|4181|
|20|6765|

**3. Table `product_parts` contains products and product parts which are needed to build them. A product part may be used to assemble another product part or product, this is stored in the `part_of_id` column. When this column contains `NULL` it means that it is the final product. List all parts and their components that are needed to build a `'chair'`.** :coffee::coffee:

>WITH RECURSIVE parts(id) as (  
SELECT id 
FROM product_parts 
WHERE name = 'chair'  
UNION ALL  
SELECT product_parts.id 
FROM product_parts, parts 
WHERE part_of_id = parts&#46;id )  
SELECT name 
FROM product_parts 
JOIN parts 
ON part_of_id = parts&#46;id;
	
**Results:**
- armrest
 - cotton
 - cushions
 - metal leg
 - metal rod
 - red dye

**4. List all bus stops between 'Zochova' and 'Zoo' for line 39. Also include the hop number on that trip between the two stops.** :coffee::coffee:

>WITH RECURSIVE hops(start_name, end_name, start_stop_id, hop) AS (  
SELECT s&#46;name, e&#46;name, s&#46;id, 1  
FROM connections c  
JOIN stops s 
ON c.start_stop_id = s&#46;id  
JOIN stops e 
ON c.end_stop_id = e&#46;id  
WHERE e&#46;name = 'Zochova' AND c&#46;line = '39'  
UNION  
SELECT s&#46;name, h.start_name, s&#46;id, h.hop+1  
FROM hops h  
JOIN connections c 
ON h.start_stop_id = c&#46;end_stop_id  
JOIN stops s 
ON c.start_stop_id = s&#46;id  
WHERE c&#46;line = '39' )  
SELECT start_name as name, hop  
FROM hops  
WHERE hop < (
SELECT hop 
FROM hops 
WHERE start_name = 'Zoo');
		
**Results:**

|stops|hops|
|---|---|
|Chatam Sófer|1|
|Park kultúry|2|
|Lafranconi|3|
