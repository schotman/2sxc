﻿using System;
using ToSic.Sxc.Code.Errors;

namespace ToSic.Sxc.Engines
{
    public class RenderingException: Exception, IExceptionWithHelp
    {
        //public RenderStatusType RenderStatus = RenderStatusType.Error;
        //public bool ShouldLog => RenderStatus != RenderStatusType.Ok;

        public RenderingException(CodeError help, string message = default) : base(message ?? help.UiMessage)
        {
            Help = help;
        }
        //public RenderingException(string message, Exception innerException) : base(message, innerException) { }
        public RenderingException(CodeError help, Exception innerException) : base("Rendering Exception",
            innerException)
        {
            Help = help;
        }

        //public RenderingException(RenderStatusType renderStat, string message): base(message)
        //{
        //    RenderStatus = renderStat;
        //}

        //public RenderingException(RenderStatusType renderStat, Exception innerException)
        //    : base("Rendering Message", innerException)
        //{
        //    RenderStatus = renderStat;
        //}

        public CodeError Help { get; }
    }
}