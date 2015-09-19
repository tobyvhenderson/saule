﻿using Saule.Serialization;
using System;
using Xunit;

namespace Tests.Serialization
{
    public class ErrorSerializerTests
    {
        [Fact(DisplayName = "Serializes common Exception properties")]
        public void SerializesProperties()
        {
            var exception = new InvalidOperationException("Some message") { HelpLink = "http://example.com" };
            var errors = new ErrorSerializer().Serialize(new ApiError(exception))["errors"][0];

            Assert.Equal(exception.Message, errors.Value<string>("title"));
            Assert.Equal(exception.HelpLink, errors["links"].Value<string>("about"));
            Assert.Equal(exception.GetType().FullName, errors.Value<string>("code"));
            Assert.Equal(exception.ToString(), errors.Value<string>("detail"));
        }

        [Fact(DisplayName = "Serializers HttpError properties")]
        public void SerializesHttpError()
        {
            var exception = new InvalidOperationException("Some message");
            var httpError = new System.Web.Http.HttpError(exception, true);

            var errors = new ErrorSerializer().Serialize(new ApiError(httpError))["errors"][0];

            Assert.Equal(httpError.ExceptionMessage, errors.Value<string>("title"));
            Assert.Equal(httpError.ExceptionType, errors.Value<string>("code"));
            Assert.Equal(httpError.StackTrace, errors.Value<string>("detail"));
        }
    }
}