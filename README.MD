# Docker

## To build

### Inside rpntest31 directory, to build api image

```
docker build -t rpntest31 .
```

### Inside rpnwebappmvc directory, to build mvc image

```
docker build -t rpnwebappmvc .
```

### Run & create api container 
```
docker run --network host --name api -d rpnrest31
```
### Run & create mvc container
```
docker run --network host --name mvc -d rpnwebappmvc
```
### To run them later:
```
docker start api
docker start mvc
```
