# Lab Notes - Assignment 10


Lab 10 - Elasticsearch
> Lenka Pejchalova

**1.  Check cluster nodes via**

```
GET 
http://localhost:9200/_cat/nodes?v
```

```
ip         heap.percent ram.percent cpu load_1m load_5m load_15m node.role master name
172.17.0.3           38          96  65    7.08    5.75     3.25 dil       -      es01
172.17.0.2           25          96  65    7.08    5.75     3.25 dil       -      es03
172.17.0.7           18          96  65    7.08    5.75     3.25 ilm       -      esmaster01
172.17.0.6           25          96  65    7.08    5.75     3.25 ilm       -      esmaster02
172.17.0.5           30          96  65    7.08    5.75     3.25 ilm       *      esmaster03
172.17.0.4           39          96  65    7.08    5.75     3.25 dil       -      es02
```

All nodes joined the cluster, master is always esmaster03.

**2.  Shut down the current master. For your convenience, the node names are the same as docker container names. To stop a node, simply stop it via docker, e.g.**

>docker stop esmaster03

```
ip         heap.percent ram.percent cpu load_1m load_5m load_15m node.role master name
172.23.0.6           27          87   8    0.84    3.04     2.19 ilm       *      esmaster02
172.23.0.2           36          87   8    0.84    3.04     2.19 dil       -      es03
172.23.0.4           22          87   8    0.84    3.04     2.19 ilm       -      esmaster01
172.23.0.3           36          87   8    0.84    3.04     2.19 dil       -      es01
172.23.0.5           40          87   7    0.84    3.04     2.19 dil       -      es02

```

After shutting esmaster3 down, new master is set - esmaster02.  

Health check:

```
GET 
http://localhost:9200/_cluster/health?pretty

```

Output:
```
{
  "cluster_name" : "es-docker-cluster",
  "status" : "green",
  "timed_out" : false,
	...
}
```

Health:   **GREEN** 

3.  While you are at this stage, with 2 masters-eligible nodes, create an index with 2 shards and 1 replica and try indexing some data. It doesn't really matter what you index, just create some index with correct settings and put in a few documents.

```
PUT localhost:9200/docs
{
    "settings": {
        "index.number_of_shards": 2,
        "index.number_of_replicas": 1
    }
}

```

Posting data like:

```
POST localhost:9200/docs/_doc
{
    "doc": 1,
    "test" : "data",
}

```

Inserts were accepted.

**4.  Stop another master-eligible node. Just to get a point across - try stopping the master-eligible node that is not currently elected master. Your cluster should now be in a bad state. Try listing all nodes via  `_cat/nodes`. Did it work?**

> docker stop esmaster01

```
GET 
http://localhost:9200/_cat/nodes?v

```

No, got following error:
```
{
    "error":{
        "root_cause":[{
            "type":"master_not_discovered_exception",
            "reason":null
            }],
        "type":"master_not_discovered_exception",
        "reason":null
        },
    "status":503
}
```


**5.  In this degraded state, try searching for data. A simple search at the  `_search`  enpoint is enough, do not bother writing a query. Does searching still work?**
    
Yes, searching works.

**6.  Bring back one of the dead master-eligible nodes**
    
> docker start esmaster01

```
ip         heap.percent ram.percent cpu load_1m load_5m load_15m node.role master name
172.23.0.6           13          88  12    0.57    0.57     1.06 ilm       -      esmaster02
172.23.0.2           36          88  12    0.57    0.57     1.06 dil       -      es03
172.23.0.5           22          88  12    0.57    0.57     1.06 dil       -      es02
172.23.0.3           33          88  12    0.57    0.57     1.06 dil       -      es01
172.23.0.4           37          88  18    0.57    0.57     1.06 ilm       *      esmaster01

```

> docker start esmaster03

```
ip         heap.percent ram.percent cpu load_1m load_5m load_15m node.role master name
172.23.0.4           27          98  86    0.63    0.55     1.01 ilm       *      esmaster01
172.23.0.6           21          98  86    0.63    0.55     1.01 ilm       -      esmaster02
172.23.0.2           18          98  86    0.63    0.55     1.01 dil       -      es03
172.23.0.5           31          98  86    0.63    0.55     1.01 dil       -      es02
172.23.0.3           16          98  86    0.63    0.55     1.01 dil       -      es01
172.23.0.7           18          98  84    0.63    0.55     1.01 ilm       -      esmaster03
```

**7.  Inspect your shard layout via**

```
curl localhost:9200/_cat/shards

```

```
data 1 p STARTED 3 8.6kb 172.23.0.2 es03
data 1 r STARTED 3 8.6kb 172.23.0.5 es02
data 0 p STARTED 1 4.3kb 172.23.0.3 es01
data 0 r STARTED 1 4.3kb 172.23.0.5 es02

```

> docker stop es02

```
data 1 p STARTED    3 8.6kb 172.23.0.2 es03
data 1 r UNASSIGNED
data 0 p STARTED    1 4.3kb 172.23.0.3 es01
data 0 r UNASSIGNED

```

Health: **YELLOW**

```
http://localhost:9200/_cat/indices

```

```
green open data 9jxU3o3XSv6vXnMRQjSXUg 2 1 4 0 26.1kb 13kb

```

Health: **GREEN**

Used shard changed to 03.


**8.  Take down another node and repeat tasks from 7. **

> docker stop es03

Health :  **YELLOW**

**9.  Try searching and indexing in this state. Are both operations working?**

Yes, both are working.
    
**10.  Take down the last data node. Access the cluster via master node, via port 9210, 9211 or 9212. What is cluster health now? Index-level health? Shard allocation?**
    
    > docker stop es01

Health:  **RED**  
Indices:  **RED**
No shards are assigned.

**11.  Bring back the data nodes one by one and observe what happens. Did you lose any data when all 3 data nodes went down?**
> docker start es01

> docker start es02

> docker start es03

Cluster's health is recovering: RED -> YELLOW -> **GREEN**
No data lost.
