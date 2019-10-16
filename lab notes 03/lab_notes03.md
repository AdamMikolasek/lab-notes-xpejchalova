# Lab Notes - Assignment 3


Lab 3 - PostGIS
> Lenka Pejchalova

**1.  How far (air distance) is FIIT STU from the Bratislava main train station? The query should output the distance in meters without any further modification.**

 >SELECT st_distance(fiit.way::geography, hsb.way::geography) 
 >FROM (SELECT * 
 >FROM planet_osm_polygon 
 >WHERE name = 'Fakulta informatiky a informačných technológií STU') as fiit, 
 >(SELECT * 
 >FROM planet_osm_polygon 
 >WHERE name = 'Hlavná stanica Bratislava') as hsb;

**Result:** 2526.187722483 m
    
**2.  Which other districts are direct neighbours with 'Karlova Ves'?**
> SELECT x&#46;name 
> FROM 
> (SELECT * 
> FROM planet_osm_polygon 
> WHERE name = 'Karlova Ves' AND admin_level = '9') as ks, 
> (SELECT * 
> FROM planet_osm_polygon 
> WHERE name != 'Karlova Ves' AND admin_level = '9') as x 
> WHERE st_touches(ks.way, x.way);

**Results**:
- Devín  
- Dúbravka  
- Bratislava - mestská časť Staré Mesto

**3.  Which bridges cross over the Danube river?**

> SELECT DISTINCT b&#46;name 
> FROM 
> (SELECT * 
> FROM planet_osm_polygon 
> WHERE name = 'Dunaj') as d, 
> (SELECT * 
> FROM planet_osm_line 
> WHERE name = 'Dunaj') as x, 
> (SELECT * 
> FROM planet_osm_line 
> WHERE bridge = 'yes' AND name IS NOT NULL) as b 
> WHERE st_intersects(d.way, b.way) OR st_intersects(x.way, b.way);

**Results**:
- Prístavný most  
- Most SNP  
- Petržalská električka - 1. časť  
- Most Lafranconi  
- Most Apollo

**4.  Find the names of all streets in 'Dlhé diely' district.**
    
> SELECT DISTINCT x&#46;name 
> FROM 
> (SELECT * 
> FROM planet_osm_polygon 
> WHERE name = 'Dlhé diely') as dd, 
> (SELECT * 
> FROM planet_osm_line 
> WHERE name IS NOT NULL) as x 
>  WHERE st_contains(dd.way, x.way);

**Results**:
- Dlhé diely II
- Vincenta Hložníka
- Jána Stanislava
- Kolískova
- Ľudovíta Fullu
- Jamnického
- Veternicová
- Beniakova
- Kresánkova
- Hlaváčikova
- Na Kampárke
- Matejkova
- Iskerníková
- Majerníkova
- Hany Meličkovej
- Tománkova
- Dlhé diely I
- Cikkerova
- Pribišova
- Vyhliadka
- Blyskáčová
- Albína Brunovského
