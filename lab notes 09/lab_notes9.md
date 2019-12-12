# Lab Notes - Assignment 9


Lab 9 - Elasticsearch
> Lenka Pejchalova


**1.  Start by writing the SQL query or queries which will select the data that you want to import to Elasticsearch. Keep in mind that you need to flatten the data, as there are no relations in Elasticsearch. Do the neccessary joins, interesction computations, etc.**

    SELECT osm_id, name, type, area
    FROM planet_osm_point pop
    WHERE area > 1000000 AND (type = 'town' OR type = 'city')

**2.  Once you have the data, design the document.**

```
{
  "name": "",
  "type": [
	"city",
	"town",
  ],
  "area": 0,
}
```

**3.  Prepare a mapping, feel free to do it "by hand", e.g. via Postman/curl, you don't need to write code for this.**

```
PUT ~/towns
{ 
	"mappings" : 
	{ 
		"properties" : 
		{ 
			"name" : {"type": "text"},
		
			"type" : { "type" : "keyword" } ,
			
			"area" : {"type": "float"}
				
		}
	}
}

```

**4.  Write a code in a language of your choice which will run the SQL queries, build JSON documents and index them in Elasticsearch. You don't need a library specifically for "Elasticsearch", just use an HTTP client library. Most of the "elasticsearch" libraries that you can find are probably overkill for this lab, as they try to provide ORM functionalities.**

I created my own program for inserting  data into Elastic using ElasticClient from Nest package. ***Source code file is attached in repository.***
There was 155 insertions in total.


**5.  You can always check the data in elasticsearch by running a simple  `GET`  request at the  `_search`  endpoint.**

GET  ~/towns/_search

```
{
    "took": 33,
    "timed_out": false,
    "_shards": {
        "total": 1,
        "successful": 1,
        "skipped": 0,
        "failed": 0
    },
    "hits": {
        "total": {
            "value": 19,
            "relation": "eq"
        },
        "max_score": 1.0,
        "hits": [
            {
                "_index": "towns",
                "_type": "_doc",
                "_id": "1t1G724B90Y1Apunp3a_",
                "_score": 1.0,
                "_source": {
                    "name": "Bratislava",
                    "type": "city",
                    "area": 879251867.4444133,
                }
            },
            {
                "_index": "towns",
                "_type": "_doc",
                "_id": "191G724B90Y1ApunqHbX",
                "_score": 1.0,
                "_source": {
                    "name": "Brezno",
                    "type": "town",
                    "area":  20862260.079674,
                }
            },
	    ...
        ]
    }
}
```

**6. Once you have the data, write the Elasticsearch query. It should be really simple, a match query with an aggregation.**
```
        GET ~/towns/_search
        {
        	"size": 0,
            "aggs" : {
                "type_types" : {
                    "terms" : { "field" : "type" } 
                }
            }
        }
        
```  
**Response:** 
```
     {
            "took": 23,
            "timed_out": false,
            "_shards": {
                "total": 1,
                "successful": 1,
                "skipped": 0,
                "failed": 0
            },
            "hits": {
                "total": {
                    "value": 155,
                    "relation": "eq"
                },
                "max_score": null,
                "hits": []
            },
            "aggregations": {
                "type_types": {
                    "doc_count_error_upper_bound": 0,
                    "sum_other_doc_count": 0,
                    "buckets": [
                        {
                            "key": "town",
                            "doc_count": 147
                        },
                        {
                            "key": "city",
                            "doc_count": 8
                        }
                    ]
                }
            }
        }
        
            
 
```

**Results according aggregation:**
 -   8 regional cities
-   147 towns


