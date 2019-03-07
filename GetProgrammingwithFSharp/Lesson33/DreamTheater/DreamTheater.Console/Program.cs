using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamTheater.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // Note: For some reason, I had to edit the project properties
            // (in Application tab) and choose to auto-generate binding redirects;
            // something to do with FSharp.Core version conflicts
            var songs = DreamTheater.Api.Songs.GetSongs();
        }
    }
}
