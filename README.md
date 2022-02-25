# DeribitFutureSubscriber
This project is composed of 3 parts :
  - Postgres DB that store and persist Future Tickers data.
  - DeribitFuturSubscriber is responsible for subscribing to Deribit web socket, listen to future ticker at 100ms and store these data in postgres DB.
  - DeribitFutureAPI expose data stored in postgres DB ex : http://localhost:8080/futureticker?name=BTC-29APR22&limit=20

Setup via Docker-compose :
  - Build solution
  - Go to project root
  - docker-compose build
  - docker-build up
  - postgres launches first then DeribitSubscribe and the API

Setup outside docker : 
  - In Connexion.cs
  - Use Localhost instead of ContainerLocalhost in ConnexionString
