﻿using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace Wallet.Repositories.Conventions
{
    public class GuidAsStringRepresentationConvention : ConventionBase, IMemberMapConvention
    {
        public void Apply(BsonMemberMap memberMap)
        {
            if (memberMap.MemberType == typeof(Guid))
            {
                memberMap.SetSerializer(
                    new GuidSerializer(BsonType.String));
            }
            else if (memberMap.MemberType == typeof(Guid?))
            {
                memberMap.SetSerializer(
                    new NullableSerializer<Guid>(new GuidSerializer(BsonType.String)));
            }
        }
    }
}
