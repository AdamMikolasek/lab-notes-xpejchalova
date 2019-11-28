# Lab Notes - Assignment 7


Lab 7 - Fulltext
> Lenka Pejchalova

**1. Implement fulltext search in contracts. The search should be case insensitive and accents insensitive. It should support searching by**

-   **contract name**
-   **department**
-   **customer**
-   **supplier**
-   **by supplier ICO**
-   **and by contract code (anywhere inside contract code, such as  `OIaMIS`)**

**Boost newer contracts, so they have a higher chance of being at top positions.**

**Try to find some limitations of your solutions - find a few queries (2 cases are enough) that are not showing expected results, or worse, are not showing any results. Think about why it's happening and propose a solution (no need to implement it).**


## Solution:
- Created extensions for **pg_trgm** and **unaccent**. 
- Created search configuration for text without diacritics and simple word forms.

>CREATE extension pg_trgm;
>CREATE extension unaccent;    

>CREATE text search configuration sk(copy = simple);  
>ALTER text search configuration sk alter mapping for word with unaccent, simple;

- Created indexing for **contracts**.

> CREATE INDEX contracts_index_vector ON contracts  
USING GIN(to_tsvector('sk', name || ' ' || department || ' ' || customer || ' ' || supplier || ' ' || identifier || ' ' || supplier_ico))

- Created trigram indexes for **supplier_ico** and **identifier** columns.

> CREATE INDEX identifier_trgm_index
ON contracts USING GIN (identifier gin_trgm_ops);

> CREATE INDEX ico_trgm_index
ON contracts USING GIN ((supplier_ico::text) gin_trgm_ops);

- Created query
>SELECT ts_rank(
>to_tsvector('sk', name || ' ' || department || ' ' || customer || ' ' || supplier || ' ' || identifier || ' '|| supplier_ico ), 
to_tsquery('sk', 'slovenska')) * (1::decimal/(extract(days from (now() - published_on))::decimal)
) AS rank, *  
FROM contracts  
WHERE to_tsvector('sk', name || ' ' || department || ' ' || customer || ' ' || supplier || ' ' || identifier || ' '|| supplier_ico ) @@ to_tsquery('sk', 'slovenska')  
ORDER BY rank DESC

## Limitations:

- Typos e.g.: 'slovenska' - 'slovemska'.
- Searching for terms within the word - solution for this could be use of  stemming or lemmatization.
