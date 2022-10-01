using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public static class PathExtensions
{
    public static TaskAwaiter<Path> GetAwaiter(this Path p)
    {
        //AstarPath.StartPath(p);

        Task<Path> t = new Task<Path>(() =>
        {
            while (!p.IsDone())
                Thread.Yield();

            return p;
        });

        t.Start();

        return t.GetAwaiter();
    }
}

