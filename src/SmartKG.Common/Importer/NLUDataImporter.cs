﻿using CommonSmartKG.Common.Data.LU;
using SmartKG.Common.Data.LU;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SmartKG.Common.Importer
{
    public class NLUDataImporter
    {
        private string rootPath;
        public NLUDataImporter(string rootPath)
        {
            this.rootPath = rootPath;
        }

        public List<NLUIntentRule> ParseIntentRules()
        {
            List<NLUIntentRule> list = new List<NLUIntentRule>();

            string intentRuleFileName = rootPath + "intentrules.tsv";

            if (!File.Exists(intentRuleFileName))
            {
                return null;
            }

            string content = File.ReadAllText(intentRuleFileName);


            string[] lines = content.Split('\n');

            int seqNo = 1;

            foreach (string line in lines)
            {
                string theLine = line.Trim();

                string[] tmps = theLine.Split('\t');

                if (tmps.Count() != 3)
                {
                    throw new Exception("invalid line in intentrules.tsv");
                }
                else
                {
                    NLUIntentRule rule = new NLUIntentRule();
                    rule.intentName = tmps[0].Trim();

                    string typeStr = tmps[1].Trim();

                    IntentRuleType type = IntentRuleType.POSITIVE;

                    if (typeStr == "NEGATIVE")
                    {
                        type = IntentRuleType.NEGATIVE;
                    }

                    rule.type = type;

                    List<string> ruleSecs = tmps[2].Trim().Split(';').ToList();

                    rule.ruleSecs = ruleSecs;

                    rule.id = seqNo.ToString();

                    seqNo += 1;

                    list.Add(rule);
                }
            }

            return list;
        }

        public List<EntityData> ParseEntityData()
        {
            List<EntityData> list = new List<EntityData>();

            string entityFileName = rootPath + "entitymap.tsv";

            if (!File.Exists(entityFileName))
            {
                return null;
            }

            string content = File.ReadAllText(entityFileName);


            string[] lines = content.Split('\n');

            int seqNo = 1;

            foreach (string line in lines)
            {

                string theLine = line.Trim();

                string[] tmps = theLine.Split('\t');

                if (tmps.Count() != 4)
                {
                    throw new Exception("invalid line in entitymap.tsv");
                }
                else
                {
                    string intentName = tmps[0].Trim();
                    string similarWord = tmps[1].Trim();

                    string entityValue = tmps[2].Trim();
                    string entityType = tmps[3].Trim();

                    EntityData entity = new EntityData();

                    entity.id = seqNo.ToString();
                    entity.intentName = intentName;
                    entity.similarWord = similarWord;
                    entity.entityValue = entityValue;
                    entity.entityType = entityType;

                    list.Add(entity);
                    seqNo += 1;
                }

            }

            return list;
        }

        public List<EntityAttributeData> ParseEntityAttributeData()
        {
            List<EntityAttributeData> list = new List<EntityAttributeData>();

            string eaFileName = rootPath + "entityAttributeMap.tsv";

            if (!File.Exists(eaFileName))
            {
                return null;
            }

            string content = File.ReadAllText(eaFileName);

            string[] lines = content.Split('\n');

            int seqNo = 1;

            foreach (string line in lines)
            {
                string theLine = line.Trim();

                if (string.IsNullOrWhiteSpace(theLine))
                {
                    continue;
                }

                string[] tmps = theLine.Split('\t');

                if (tmps.Count() != 5)
                {
                    throw new Exception("invalid line in entityAttributeMap.tsv");
                }
                else
                {
                    string intentName = tmps[0];
                    string entityValue = tmps[1];
                    string entityType = tmps[2];
                    string attributeName = tmps[3];
                    string attributeValue = tmps[4];

                    EntityAttributeData ea = new EntityAttributeData();
                    ea.id = seqNo.ToString();
                    ea.intentName = intentName;
                    ea.entityValue = entityValue;
                    ea.entityType = entityType;
                    ea.attributeName = attributeName;
                    ea.attributeValue = attributeValue;

                    list.Add(ea);
                    seqNo += 1;
                }
            }

            return list;
        }
    }
}