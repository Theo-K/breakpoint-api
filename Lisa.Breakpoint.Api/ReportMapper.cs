﻿using Lisa.Common.TableStorage;
using Lisa.Common.WebApi;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Lisa.Breakpoint.Api
{
    public class ReportMapper
    {
        public static ITableEntity ToEntity(dynamic model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            dynamic entity = new DynamicEntity();
            entity.Title = model.Title;
            entity.Project = model.Project;
            entity.Assignee = model.Assignee;
            entity.Status = model.Status;

            dynamic metadata = model.GetMetadata();
            if (metadata == null)
            {
                entity.Id = Guid.NewGuid();
                entity.Reported = DateTime.UtcNow;
                entity.Status = "open";
            }
            else
            {
                entity.Id = model.Id;
                entity.Reported = model.Reported;
                entity.PartitionKey = metadata.PartitionKey;
                entity.RowKey = metadata.RowKey;
            }

            return entity;
        }

        public static DynamicModel ToModel(dynamic entity)
        {
            if (entity == null)
            {
                return null;
            }

            dynamic model = new DynamicModel();
            model.Id = entity.Id;
            model.Title = entity.Title;
            model.Project = entity.Project;
            if (entity.Assignee != null)
            {
                model.Assignee = entity.Assignee;
            }
            model.Status = entity.Status;
            model.Reported = entity.Reported;

            var metadata = new
            {
                PartitionKey = entity.PartitionKey,
                RowKey = entity.RowKey
            };
            model.SetMetadata(metadata);

            return model;
        }
    }
}