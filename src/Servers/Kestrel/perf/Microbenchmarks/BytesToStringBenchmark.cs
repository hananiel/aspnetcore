// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;
using Microsoft.Net.Http.Headers;

namespace Microsoft.AspNetCore.Server.Kestrel.Microbenchmarks;

public class BytesToStringBenchmark
{
    private const int Iterations = 50;

    private string _headerName;
    private byte[] _asciiBytes;
    private byte[] _utf8Bytes;

    [Params(
        BenchmarkTypes.KeepAlive,
        BenchmarkTypes.Accept,
        BenchmarkTypes.UserAgent,
        BenchmarkTypes.Cookie
    )]
    public BenchmarkTypes Type { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        switch (Type)
        {
            case BenchmarkTypes.KeepAlive:
                _headerName = HeaderNames.Connection;
                // keep-alive
                _asciiBytes = new byte[] { 0x6b, 0x65, 0x65, 0x70, 0x2d, 0x61, 0x6c, 0x69, 0x76, 0x65 };
                // kéép-álivé
                _utf8Bytes = new byte[] { 0x6b, 0xc3, 0xa9, 0xc3, 0xa9, 0x70, 0x2d, 0xc3, 0xa1, 0x6c, 0x69, 0x76, 0xc3, 0xa9 };
                break;
            case BenchmarkTypes.Accept:
                _headerName = HeaderNames.Accept;
                // text/plain,text/html;q=0.9,application/xhtml+xml;q=0.9,application/xml;q=0.8,*/*;q=0.7
                _asciiBytes = new byte[] { 0x74, 0x65, 0x78, 0x74, 0x2f, 0x70, 0x6c, 0x61, 0x69, 0x6e, 0x2c, 0x74, 0x65, 0x78, 0x74, 0x2f, 0x68, 0x74, 0x6d, 0x6c, 0x3b, 0x71, 0x3d, 0x30, 0x2e, 0x39, 0x2c, 0x61, 0x70, 0x70, 0x6c, 0x69, 0x63, 0x61, 0x74, 0x69, 0x6f, 0x6e, 0x2f, 0x78, 0x68, 0x74, 0x6d, 0x6c, 0x2b, 0x78, 0x6d, 0x6c, 0x3b, 0x71, 0x3d, 0x30, 0x2e, 0x39, 0x2c, 0x61, 0x70, 0x70, 0x6c, 0x69, 0x63, 0x61, 0x74, 0x69, 0x6f, 0x6e, 0x2f, 0x78, 0x6d, 0x6c, 0x3b, 0x71, 0x3d, 0x30, 0x2e, 0x38, 0x2c, 0x2a, 0x2f, 0x2a, 0x3b, 0x71, 0x3d, 0x30, 0x2e, 0x37 };
                // téxt/pláin,téxt/html;q=0.9,ápplicátion/xhtml+xml;q=0.9,ápplicátion/xml;q=0.8,*/*;q=0.7
                _utf8Bytes = new byte[] { 0x74, 0xc3, 0xa9, 0x78, 0x74, 0x2f, 0x70, 0x6c, 0xc3, 0xa1, 0x69, 0x6e, 0x2c, 0x74, 0xc3, 0xa9, 0x78, 0x74, 0x2f, 0x68, 0x74, 0x6d, 0x6c, 0x3b, 0x71, 0x3d, 0x30, 0x2e, 0x39, 0x2c, 0xc3, 0xa1, 0x70, 0x70, 0x6c, 0x69, 0x63, 0xc3, 0xa1, 0x74, 0x69, 0x6f, 0x6e, 0x2f, 0x78, 0x68, 0x74, 0x6d, 0x6c, 0x2b, 0x78, 0x6d, 0x6c, 0x3b, 0x71, 0x3d, 0x30, 0x2e, 0x39, 0x2c, 0xc3, 0xa1, 0x70, 0x70, 0x6c, 0x69, 0x63, 0xc3, 0xa1, 0x74, 0x69, 0x6f, 0x6e, 0x2f, 0x78, 0x6d, 0x6c, 0x3b, 0x71, 0x3d, 0x30, 0x2e, 0x38, 0x2c, 0x2a, 0x2f, 0x2a, 0x3b, 0x71, 0x3d, 0x30, 0x2e, 0x37 };
                break;
            case BenchmarkTypes.UserAgent:
                _headerName = HeaderNames.UserAgent;
                // Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36
                _asciiBytes = new byte[] { 0x4d, 0x6f, 0x7a, 0x69, 0x6c, 0x6c, 0x61, 0x2f, 0x35, 0x2e, 0x30, 0x20, 0x28, 0x57, 0x69, 0x6e, 0x64, 0x6f, 0x77, 0x73, 0x20, 0x4e, 0x54, 0x20, 0x31, 0x30, 0x2e, 0x30, 0x3b, 0x20, 0x57, 0x4f, 0x57, 0x36, 0x34, 0x29, 0x20, 0x41, 0x70, 0x70, 0x6c, 0x65, 0x57, 0x65, 0x62, 0x4b, 0x69, 0x74, 0x2f, 0x35, 0x33, 0x37, 0x2e, 0x33, 0x36, 0x20, 0x28, 0x4b, 0x48, 0x54, 0x4d, 0x4c, 0x2c, 0x20, 0x6c, 0x69, 0x6b, 0x65, 0x20, 0x47, 0x65, 0x63, 0x6b, 0x6f, 0x29, 0x20, 0x43, 0x68, 0x72, 0x6f, 0x6d, 0x65, 0x2f, 0x35, 0x34, 0x2e, 0x30, 0x2e, 0x32, 0x38, 0x34, 0x30, 0x2e, 0x39, 0x39, 0x20, 0x53, 0x61, 0x66, 0x61, 0x72, 0x69, 0x2f, 0x35, 0x33, 0x37, 0x2e, 0x33, 0x36 };
                // Mozillá/5.0 (Windows NT 10.0; WOW64) áppléWébKit/537.36 (KHTML, liké Gécko) Chromé/54.0.2840.99 Sáfári/537.36
                _utf8Bytes = new byte[] { 0x4d, 0x6f, 0x7a, 0x69, 0x6c, 0x6c, 0xc3, 0xa1, 0x2f, 0x35, 0x2e, 0x30, 0x20, 0x28, 0x57, 0x69, 0x6e, 0x64, 0x6f, 0x77, 0x73, 0x20, 0x4e, 0x54, 0x20, 0x31, 0x30, 0x2e, 0x30, 0x3b, 0x20, 0x57, 0x4f, 0x57, 0x36, 0x34, 0x29, 0x20, 0xc3, 0xa1, 0x70, 0x70, 0x6c, 0xc3, 0xa9, 0x57, 0xc3, 0xa9, 0x62, 0x4b, 0x69, 0x74, 0x2f, 0x35, 0x33, 0x37, 0x2e, 0x33, 0x36, 0x20, 0x28, 0x4b, 0x48, 0x54, 0x4d, 0x4c, 0x2c, 0x20, 0x6c, 0x69, 0x6b, 0xc3, 0xa9, 0x20, 0x47, 0xc3, 0xa9, 0x63, 0x6b, 0x6f, 0x29, 0x20, 0x43, 0x68, 0x72, 0x6f, 0x6d, 0xc3, 0xa9, 0x2f, 0x35, 0x34, 0x2e, 0x30, 0x2e, 0x32, 0x38, 0x34, 0x30, 0x2e, 0x39, 0x39, 0x20, 0x53, 0xc3, 0xa1, 0x66, 0xc3, 0xa1, 0x72, 0x69, 0x2f, 0x35, 0x33, 0x37, 0x2e, 0x33, 0x36 };
                break;
            case BenchmarkTypes.Cookie:
                _headerName = HeaderNames.Cookie;
                // prov=20629ccd-8b0f-e8ef-2935-cd26609fc0bc; __qca=P0-1591065732-1479167353442; _ga=GA1.2.1298898376.1479167354; _gat=1; sgt=id=9519gfde_3347_4762_8762_df51458c8ec2; acct=t=why-is-%e0%a5%a7%e0%a5%a8%e0%a5%a9-numeric&s=why-is-%e0%a5%a7%e0%a5%a8%e0%a5%a9-numeric
                _asciiBytes = new byte[] { 0x70, 0x72, 0x6f, 0x76, 0x3d, 0x32, 0x30, 0x36, 0x32, 0x39, 0x63, 0x63, 0x64, 0x2d, 0x38, 0x62, 0x30, 0x66, 0x2d, 0x65, 0x38, 0x65, 0x66, 0x2d, 0x32, 0x39, 0x33, 0x35, 0x2d, 0x63, 0x64, 0x32, 0x36, 0x36, 0x30, 0x39, 0x66, 0x63, 0x30, 0x62, 0x63, 0x3b, 0x20, 0x5f, 0x5f, 0x71, 0x63, 0x61, 0x3d, 0x50, 0x30, 0x2d, 0x31, 0x35, 0x39, 0x31, 0x30, 0x36, 0x35, 0x37, 0x33, 0x32, 0x2d, 0x31, 0x34, 0x37, 0x39, 0x31, 0x36, 0x37, 0x33, 0x35, 0x33, 0x34, 0x34, 0x32, 0x3b, 0x20, 0x5f, 0x67, 0x61, 0x3d, 0x47, 0x41, 0x31, 0x2e, 0x32, 0x2e, 0x31, 0x32, 0x39, 0x38, 0x38, 0x39, 0x38, 0x33, 0x37, 0x36, 0x2e, 0x31, 0x34, 0x37, 0x39, 0x31, 0x36, 0x37, 0x33, 0x35, 0x34, 0x3b, 0x20, 0x5f, 0x67, 0x61, 0x74, 0x3d, 0x31, 0x3b, 0x20, 0x73, 0x67, 0x74, 0x3d, 0x69, 0x64, 0x3d, 0x39, 0x35, 0x31, 0x39, 0x67, 0x66, 0x64, 0x65, 0x5f, 0x33, 0x33, 0x34, 0x37, 0x5f, 0x34, 0x37, 0x36, 0x32, 0x5f, 0x38, 0x37, 0x36, 0x32, 0x5f, 0x64, 0x66, 0x35, 0x31, 0x34, 0x35, 0x38, 0x63, 0x38, 0x65, 0x63, 0x32, 0x3b, 0x20, 0x61, 0x63, 0x63, 0x74, 0x3d, 0x74, 0x3d, 0x77, 0x68, 0x79, 0x2d, 0x69, 0x73, 0x2d, 0x25, 0x65, 0x30, 0x25, 0x61, 0x35, 0x25, 0x61, 0x37, 0x25, 0x65, 0x30, 0x25, 0x61, 0x35, 0x25, 0x61, 0x38, 0x25, 0x65, 0x30, 0x25, 0x61, 0x35, 0x25, 0x61, 0x39, 0x2d, 0x6e, 0x75, 0x6d, 0x65, 0x72, 0x69, 0x63, 0x26, 0x73, 0x3d, 0x77, 0x68, 0x79, 0x2d, 0x69, 0x73, 0x2d, 0x25, 0x65, 0x30, 0x25, 0x61, 0x35, 0x25, 0x61, 0x37, 0x25, 0x65, 0x30, 0x25, 0x61, 0x35, 0x25, 0x61, 0x38, 0x25, 0x65, 0x30, 0x25, 0x61, 0x35, 0x25, 0x61, 0x39, 0x2d, 0x6e, 0x75, 0x6d, 0x65, 0x72, 0x69, 0x63 };
                // prov=20629ccd-8b0f-é8éf-2935-cd26609fc0bc; __qcá=P0-1591065732-1479167353442; _gá=Gá1.2.1298898376.1479167354; _gát=1; sgt=id=9519gfdé_3347_4762_8762_df51458c8éc2; ácct=t=why-is-%é0%á5%á7%é0%á5%á8%é0%á5%á9-numéric&s=why-is-%é0%á5%á7%é0%á5%á8%é0%á5%á9-numéric
                _utf8Bytes = new byte[] { 0x70, 0x72, 0x6f, 0x76, 0x3d, 0x32, 0x30, 0x36, 0x32, 0x39, 0x63, 0x63, 0x64, 0x2d, 0x38, 0x62, 0x30, 0x66, 0x2d, 0xc3, 0xa9, 0x38, 0xc3, 0xa9, 0x66, 0x2d, 0x32, 0x39, 0x33, 0x35, 0x2d, 0x63, 0x64, 0x32, 0x36, 0x36, 0x30, 0x39, 0x66, 0x63, 0x30, 0x62, 0x63, 0x3b, 0x20, 0x5f, 0x5f, 0x71, 0x63, 0xc3, 0xa1, 0x3d, 0x50, 0x30, 0x2d, 0x31, 0x35, 0x39, 0x31, 0x30, 0x36, 0x35, 0x37, 0x33, 0x32, 0x2d, 0x31, 0x34, 0x37, 0x39, 0x31, 0x36, 0x37, 0x33, 0x35, 0x33, 0x34, 0x34, 0x32, 0x3b, 0x20, 0x5f, 0x67, 0xc3, 0xa1, 0x3d, 0x47, 0xc3, 0xa1, 0x31, 0x2e, 0x32, 0x2e, 0x31, 0x32, 0x39, 0x38, 0x38, 0x39, 0x38, 0x33, 0x37, 0x36, 0x2e, 0x31, 0x34, 0x37, 0x39, 0x31, 0x36, 0x37, 0x33, 0x35, 0x34, 0x3b, 0x20, 0x5f, 0x67, 0xc3, 0xa1, 0x74, 0x3d, 0x31, 0x3b, 0x20, 0x73, 0x67, 0x74, 0x3d, 0x69, 0x64, 0x3d, 0x39, 0x35, 0x31, 0x39, 0x67, 0x66, 0x64, 0xc3, 0xa9, 0x5f, 0x33, 0x33, 0x34, 0x37, 0x5f, 0x34, 0x37, 0x36, 0x32, 0x5f, 0x38, 0x37, 0x36, 0x32, 0x5f, 0x64, 0x66, 0x35, 0x31, 0x34, 0x35, 0x38, 0x63, 0x38, 0xc3, 0xa9, 0x63, 0x32, 0x3b, 0x20, 0xc3, 0xa1, 0x63, 0x63, 0x74, 0x3d, 0x74, 0x3d, 0x77, 0x68, 0x79, 0x2d, 0x69, 0x73, 0x2d, 0x25, 0xc3, 0xa9, 0x30, 0x25, 0xc3, 0xa1, 0x35, 0x25, 0xc3, 0xa1, 0x37, 0x25, 0xc3, 0xa9, 0x30, 0x25, 0xc3, 0xa1, 0x35, 0x25, 0xc3, 0xa1, 0x38, 0x25, 0xc3, 0xa9, 0x30, 0x25, 0xc3, 0xa1, 0x35, 0x25, 0xc3, 0xa1, 0x39, 0x2d, 0x6e, 0x75, 0x6d, 0xc3, 0xa9, 0x72, 0x69, 0x63, 0x26, 0x73, 0x3d, 0x77, 0x68, 0x79, 0x2d, 0x69, 0x73, 0x2d, 0x25, 0xc3, 0xa9, 0x30, 0x25, 0xc3, 0xa1, 0x35, 0x25, 0xc3, 0xa1, 0x37, 0x25, 0xc3, 0xa9, 0x30, 0x25, 0xc3, 0xa1, 0x35, 0x25, 0xc3, 0xa1, 0x38, 0x25, 0xc3, 0xa9, 0x30, 0x25, 0xc3, 0xa1, 0x35, 0x25, 0xc3, 0xa1, 0x39, 0x2d, 0x6e, 0x75, 0x6d, 0xc3, 0xa9, 0x72, 0x69, 0x63 };
                break;
        }
    }

    [Benchmark(Baseline = true, OperationsPerInvoke = Iterations)]
    public void AsciiBytesToString()
    {
        for (uint i = 0; i < Iterations; i++)
        {
            HttpUtilities.GetAsciiString(_asciiBytes);
        }
    }

    [Benchmark(OperationsPerInvoke = Iterations)]
    public void Utf8BytesToString()
    {
        for (uint i = 0; i < Iterations; i++)
        {
            HttpUtilities.GetRequestHeaderString(_utf8Bytes, _headerName, KestrelServerOptions.DefaultHeaderEncodingSelector, checkForNewlineChars: true);
        }
    }

    public enum BenchmarkTypes
    {
        KeepAlive,
        Accept,
        UserAgent,
        Cookie,
    }
}
