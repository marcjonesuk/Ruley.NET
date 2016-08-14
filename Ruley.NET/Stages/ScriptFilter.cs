﻿using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Newtonsoft.Json;
using Ruley.Core.Outputs;

namespace Ruley.Core.Filters
{
    public class ScriptStage : InlineStage
    {
        [JsonRequired]
        [Primary]
        public Property<string> Value { get; set; }
        private Script<object> _script = null;
 
        private object _sync = new object();

        public override Event Apply(Event msg)
        {
            lock (_sync)
            {
                if (_script == null)
                {
                    var scriptOptions = ScriptOptions.Default
                        .WithImports("System")
                        .WithReferences("Microsoft.CSharp");

                    _script = CSharpScript.Create<object>(Value.Get(msg), globalsType: typeof(Globals), options: scriptOptions);
                }
            }

            var g = new Globals();
            g.e = msg.Data;
            g.p = msg.Parameters;
            _script.RunAsync(g).Wait();

            return msg;
        }
    }
}