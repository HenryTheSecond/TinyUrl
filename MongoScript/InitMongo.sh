echo "Initializing MongoDB replica set..."
mongosh --eval "rs.initiate({_id: \"myReplicaSet\",members: [{_id: 0, host: \"MongoDb\"}]})"