
# Lab Notes - Assignment 8


Lab 8 - ACID
> Lenka Pejchalova

I created my own benchmarking insertion script. Source code files are attached in repository.

**1. Try benchmarking PostgreSQL write performance with and without synchronous_commit option set. The metric that you should measure is inserts per seconds - i.e., how many inserts can the database process in 1 second.**

- **insertion of 1 000 values with synchronous_commit=on**

> docker run -p 5432:5432 fiitpdt/postgres

Statistics of insertions
Number of inserts: 1000  
Duration: 1.7538942  
Inserts per seconds: 570.15981921828  


- **insertion of 1 000 values with synchronous_commit=off**


> docker run -p 5432:5432 fiitpdt/postgres postgres -c 'synchronous_commit=off'

Statistics of insertions
Number of inserts: 1000
Duration: 1.5842787
Inserts per seconds: 631.20207322108

**2. Now, run the benchmark against the stock docker image. How many inserts per second did it handle?**

- **insertion of 10 000 values with synchronous_commit=on**

> docker run -p 5432:5432 fiitpdt/postgres

Statistics of insertions
Number of inserts: 10000
Duration: 8.6418776
Inserts per seconds: 1157.15594027853

- **insertion of 10 000 values with synchronous_commit=off**

> docker run -p 5432:5432 fiitpdt/postgres postgres -c 'synchronous_commit=off'

Statistics of insertions
Number of inserts: 10000
Duration: 5.2470754
Inserts per seconds: 1905.82357554839


**3. Run the benchmark again. How many inserts per second can it handle now?**

- **insertion of 20 000 values with synchronous_commit=on** 

>  docker run -p 5432:5432 fiitpdt/postgres

Statistics of insertions
Number of inserts: 20000
Duration: 17.46793576
Inserts per seconds: 1144.95497778267

- **insertion of 20 000 values with synchronous_commit=off**

> docker run -p 5432:5432 fiitpdt/postgres postgres -c 'synchronous_commit=off'

Statistics of insertions
Number of inserts: 20000
Duration: 12.53285389
Inserts per seconds: 1595.80572593749


### Summary:
There is a visible improvement with a greater number of inserts.
