# Lab Notes - Assignment 5


Lab 5 - PostGIS
> Lenka Pejchalova

**1.  Write a query which finds all restaurants (point, `amenity = 'restaurant'`) within 1000 meters of 'Fakulta informatiky a informačných technológií STU' (polygon). Select the restaurant name and distance in meters. Sort the output by distance - closest restaurant first.**

>WITH restaurants AS (  
SELECT * 
FROM planet_osm_point  
WHERE amenity = 'restaurant' )  
SELECT restaurants&#46;name, ST_DISTANCE(restaurants.way::geography, polygons.way::geography) AS distance 
FROM restaurants  
CROSS JOIN planet_osm_polygon polygons  
WHERE polygons&#46;name = 'Fakulta informatiky a informačných technológií STU'  
AND ST_DWITHIN(restaurants.way::geography, polygons.way::geography, 1000)  
ORDER BY distance

**Results:** 

|name|distance|
|--|--|
|Bastion - Slovenská Koliba|35.742375541|
|Drag| 366.312228092|
|Idyla|414.046695922|
|Družba|622.942884737|
|Venza (študentská jedáleň)|674.141796757|
|Kamel|749.299621785|
|Eat and Meet (študentská jedáleň)|771.717881845|
|Riviera|825.566535564|
|Seoul garden|833.397496062|



**2. Check the query plan and measure how long the query takes. Now make it as fast as possible. Make sure to also use geo-indices, but don't expect large improvements. The dataset is small, and filtering in  `amenity='restaurant'`  will greatly limit the search space anyway.**

**Result without indexes:** 440 msec
> CREATE INDEX index_point 
> ON planet_osm_point USING GIST((way::geography))

>CREATE INDEX index_polygon 
>ON planet_osm_polygon USING GIST((way::geography))

**Result after indexing:**  282 msec

**3. 1.  Update the query to generate a geojson. The output should be a single row containg a json array like this:**

> WITH restaurants AS (  
SELECT * 
FROM planet_osm_point  
WHERE amenity = 'restaurant' ) 
SELECT array_to_json (array_agg((row_to_json(json))::JSON)) 
FROM (  
SELECT restaurants&#46;name, ST_DISTANCE(restaurants.way::geography, polygons.way::geography) AS distance 
FROM restaurants  
CROSS JOIN planet_osm_polygon polygons  
WHERE polygons&#46;name = 'Fakulta informatiky a informačných technológií STU'  
AND ST_DWITHIN(restaurants.way::geography, polygons.way::geography, 1000)  
ORDER BY distance) AS json



**Results:**
```json
[{"name":"Bastion - Slovenská Koliba","dist":35.742375541,"geo":{"type":"Point","coordinates":[17.0722601,48.1547198]}},{"name":"Drag","dist":366.312228092,"geo":{"type":"Point","coordinates":[17.0672632,48.1509517]}},{"name":"Idyla","dist":414.046695922,"geo":{"type":"Point","coordinates":[17.0779226,48.1538106]}},{"name":"Družba","dist":622.942884737,"geo":{"type":"Point","coordinates":[17.0701073,48.147537]}},{"name":"Venza (študentská jedáleň)","dist":674.141796757,"geo":{"type":"Point","coordinates":[17.0690275,48.1605604]}},{"name":"Kamel","dist":749.299621785,"geo":{"type":"Point","coordinates":[17.0618877,48.150218]}},{"name":"Eat and Meet (študentská jedáleň)","dist":771.717881845,"geo":{"type":"Point","coordinates":[17.0672253,48.1610779]}},{"name":"Riviera","dist":825.566535564,"geo":{"type":"Point","coordinates":[17.0629032,48.1480213]}},{"name":"Seoul garden","dist":833.397496062,"geo":{"type":"Point","coordinates":[17.0772523,48.1610729]}}]```
