﻿using System.Runtime.InteropServices;

namespace JellyBins.NewExecutable.Headers;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct NeModule
{
    public UInt32 ImportOffset;
}