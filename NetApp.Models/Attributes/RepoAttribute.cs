using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Models.Attributes
{
    public enum RepoTypeEnum
    {
        SQLite = 0x1,
        MySQL = 0x10,
        Redis = 0x100,
        InMemory = 0x1000
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RepoAttribute : Attribute
    {
        private RepoTypeEnum _repoType;
        public RepoAttribute(RepoTypeEnum repoType)
        {
            _repoType = repoType;
        }

        public RepoTypeEnum RepoType => _repoType;
    }
}