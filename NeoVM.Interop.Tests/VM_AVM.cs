using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoVM.Interop.Enums;
using NeoVM.Interop.Tests.Extra;
using NeoVM.Interop.Types;
using NeoVM.Interop.Types.Arguments;
using NeoVM.Interop.Types.StackItems;
using System;

namespace NeoVM.Interop.Tests
{
    [TestClass]
    public class VM_AVM : VMOpCodeTest
    {
        /* Length: 000006E1 */

        byte[] script = {
    0x5A, 0xC5, 0x6B, 0x6A, 0x00, 0x52, 0x7A, 0xC4, 0x6A, 0x51, 0x52, 0x7A,
    0xC4, 0x6A, 0x00, 0xC3, 0x11, 0x72, 0x65, 0x67, 0x69, 0x73, 0x74, 0x65,
    0x72, 0x5F, 0x70, 0x72, 0x6F, 0x70, 0x6F, 0x73, 0x61, 0x6C, 0x9C, 0x64,
    0x0C, 0x00, 0x6A, 0x51, 0xC3, 0x65, 0x23, 0x04, 0x6C, 0x75, 0x66, 0x61,
    0x6A, 0x00, 0xC3, 0x04, 0x76, 0x6F, 0x74, 0x65, 0x9C, 0x64, 0x0C, 0x00,
    0x6A, 0x51, 0xC3, 0x65, 0x94, 0x01, 0x6C, 0x75, 0x66, 0x61, 0x6A, 0x00,
    0xC3, 0x05, 0x63, 0x6F, 0x75, 0x6E, 0x74, 0x9C, 0x64, 0x0C, 0x00, 0x6A,
    0x51, 0xC3, 0x65, 0x0B, 0x00, 0x6C, 0x75, 0x66, 0x61, 0x00, 0x6C, 0x75,
    0x66, 0x01, 0x12, 0xC5, 0x6B, 0x6A, 0x00, 0x52, 0x7A, 0xC4, 0x68, 0x16,
    0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E,
    0x47, 0x65, 0x74, 0x43, 0x6F, 0x6E, 0x74, 0x65, 0x78, 0x74, 0x61, 0x6A,
    0x51, 0x52, 0x7A, 0xC4, 0x6A, 0x00, 0xC3, 0xC0, 0x51, 0x9E, 0x64, 0x07,
    0x00, 0x00, 0x6C, 0x75, 0x66, 0x61, 0x6A, 0x00, 0xC3, 0x00, 0xC3, 0x65,
    0x69, 0x05, 0x6A, 0x52, 0x52, 0x7A, 0xC4, 0x6A, 0x51, 0xC3, 0x6A, 0x52,
    0xC3, 0x02, 0x69, 0x64, 0xC3, 0x7C, 0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E,
    0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x47, 0x65, 0x74, 0x61,
    0x63, 0x2B, 0x00, 0x12, 0x50, 0x72, 0x6F, 0x70, 0x6F, 0x73, 0x61, 0x6C,
    0x20, 0x6E, 0x6F, 0x74, 0x20, 0x66, 0x6F, 0x75, 0x6E, 0x64, 0x68, 0x0F,
    0x4E, 0x65, 0x6F, 0x2E, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65, 0x2E,
    0x4C, 0x6F, 0x67, 0x00, 0x6C, 0x75, 0x66, 0x61, 0x6A, 0x51, 0xC3, 0x6A,
    0x52, 0xC3, 0x04, 0x74, 0x65, 0x78, 0x74, 0xC3, 0x7C, 0x68, 0x0F, 0x4E,
    0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x47,
    0x65, 0x74, 0x61, 0x6A, 0x53, 0x52, 0x7A, 0xC4, 0x6A, 0x53, 0xC3, 0x68,
    0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65,
    0x2E, 0x4C, 0x6F, 0x67, 0x6A, 0x51, 0xC3, 0x6A, 0x52, 0xC3, 0x03, 0x79,
    0x65, 0x73, 0xC3, 0x7C, 0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74,
    0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x47, 0x65, 0x74, 0x61, 0x6A, 0x54,
    0x52, 0x7A, 0xC4, 0x6A, 0x51, 0xC3, 0x6A, 0x52, 0xC3, 0x02, 0x6E, 0x6F,
    0xC3, 0x7C, 0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72,
    0x61, 0x67, 0x65, 0x2E, 0x47, 0x65, 0x74, 0x61, 0x6A, 0x55, 0x52, 0x7A,
    0xC4, 0x6A, 0x54, 0xC3, 0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x52, 0x75,
    0x6E, 0x74, 0x69, 0x6D, 0x65, 0x2E, 0x4C, 0x6F, 0x67, 0x6A, 0x55, 0xC3,
    0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D,
    0x65, 0x2E, 0x4C, 0x6F, 0x67, 0x6A, 0x00, 0xC3, 0x00, 0xC3, 0x6A, 0x53,
    0xC3, 0x6A, 0x54, 0xC3, 0x6A, 0x55, 0xC3, 0x53, 0x79, 0x51, 0x79, 0x55,
    0x72, 0x75, 0x51, 0x72, 0x75, 0x52, 0x79, 0x52, 0x79, 0x54, 0x72, 0x75,
    0x52, 0x72, 0x75, 0x05, 0x63, 0x6F, 0x75, 0x6E, 0x74, 0x55, 0xC1, 0x68,
    0x12, 0x4E, 0x65, 0x6F, 0x2E, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65,
    0x2E, 0x4E, 0x6F, 0x74, 0x69, 0x66, 0x79, 0x51, 0x6C, 0x75, 0x66, 0x01,
    0x1E, 0xC5, 0x6B, 0x6A, 0x00, 0x52, 0x7A, 0xC4, 0x68, 0x16, 0x4E, 0x65,
    0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x47, 0x65,
    0x74, 0x43, 0x6F, 0x6E, 0x74, 0x65, 0x78, 0x74, 0x61, 0x6A, 0x51, 0x52,
    0x7A, 0xC4, 0x6A, 0x00, 0xC3, 0xC0, 0x53, 0x9E, 0x64, 0x07, 0x00, 0x00,
    0x6C, 0x75, 0x66, 0x61, 0x6A, 0x00, 0xC3, 0x00, 0xC3, 0x65, 0xF7, 0x03,
    0x6A, 0x52, 0x52, 0x7A, 0xC4, 0x6A, 0x51, 0xC3, 0x6A, 0x52, 0xC3, 0x02,
    0x69, 0x64, 0xC3, 0x7C, 0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74,
    0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x47, 0x65, 0x74, 0x61, 0x63, 0x2B,
    0x00, 0x12, 0x50, 0x72, 0x6F, 0x70, 0x6F, 0x73, 0x61, 0x6C, 0x20, 0x6E,
    0x6F, 0x74, 0x20, 0x66, 0x6F, 0x75, 0x6E, 0x64, 0x68, 0x0F, 0x4E, 0x65,
    0x6F, 0x2E, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65, 0x2E, 0x4C, 0x6F,
    0x67, 0x00, 0x6C, 0x75, 0x66, 0x61, 0x6A, 0x00, 0xC3, 0x51, 0xC3, 0x68,
    0x18, 0x4E, 0x65, 0x6F, 0x2E, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65,
    0x2E, 0x43, 0x68, 0x65, 0x63, 0x6B, 0x57, 0x69, 0x74, 0x6E, 0x65, 0x73,
    0x73, 0x61, 0x63, 0x30, 0x00, 0x17, 0x59, 0x6F, 0x75, 0x20, 0x61, 0x72,
    0x65, 0x20, 0x6E, 0x6F, 0x74, 0x20, 0x77, 0x68, 0x6F, 0x20, 0x79, 0x6F,
    0x75, 0x20, 0x73, 0x61, 0x79, 0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x52,
    0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65, 0x2E, 0x4C, 0x6F, 0x67, 0x00, 0x6C,
    0x75, 0x66, 0x61, 0x6A, 0x51, 0xC3, 0x6A, 0x52, 0xC3, 0x04, 0x61, 0x64,
    0x64, 0x72, 0xC3, 0x6A, 0x00, 0xC3, 0x51, 0xC3, 0x7E, 0x7C, 0x68, 0x0F,
    0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E,
    0x47, 0x65, 0x74, 0x61, 0x6A, 0x53, 0x52, 0x7A, 0xC4, 0x6A, 0x53, 0xC3,
    0x63, 0x2C, 0x00, 0x13, 0x4E, 0x6F, 0x74, 0x20, 0x61, 0x6C, 0x6C, 0x6F,
    0x77, 0x65, 0x64, 0x20, 0x74, 0x6F, 0x20, 0x76, 0x6F, 0x74, 0x65, 0x68,
    0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65,
    0x2E, 0x4C, 0x6F, 0x67, 0x00, 0x6C, 0x75, 0x66, 0x61, 0x6A, 0x53, 0xC3,
    0x52, 0x9C, 0x64, 0x26, 0x00, 0x0D, 0x41, 0x6C, 0x72, 0x65, 0x61, 0x64,
    0x79, 0x20, 0x76, 0x6F, 0x74, 0x65, 0x64, 0x68, 0x0F, 0x4E, 0x65, 0x6F,
    0x2E, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65, 0x2E, 0x4C, 0x6F, 0x67,
    0x00, 0x6C, 0x75, 0x66, 0x61, 0x6A, 0x00, 0xC3, 0x52, 0xC3, 0x51, 0x9C,
    0x64, 0x63, 0x00, 0x04, 0x59, 0x65, 0x73, 0x21, 0x68, 0x0F, 0x4E, 0x65,
    0x6F, 0x2E, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65, 0x2E, 0x4C, 0x6F,
    0x67, 0x6A, 0x51, 0xC3, 0x6A, 0x52, 0xC3, 0x03, 0x79, 0x65, 0x73, 0xC3,
    0x7C, 0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61,
    0x67, 0x65, 0x2E, 0x47, 0x65, 0x74, 0x61, 0x6A, 0x54, 0x52, 0x7A, 0xC4,
    0x6A, 0x51, 0xC3, 0x6A, 0x52, 0xC3, 0x03, 0x79, 0x65, 0x73, 0xC3, 0x6A,
    0x54, 0xC3, 0x51, 0x93, 0x52, 0x72, 0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E,
    0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x50, 0x75, 0x74, 0x61,
    0x62, 0x5E, 0x00, 0x61, 0x03, 0x4E, 0x6F, 0x21, 0x68, 0x0F, 0x4E, 0x65,
    0x6F, 0x2E, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65, 0x2E, 0x4C, 0x6F,
    0x67, 0x6A, 0x51, 0xC3, 0x6A, 0x52, 0xC3, 0x02, 0x6E, 0x6F, 0xC3, 0x7C,
    0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67,
    0x65, 0x2E, 0x47, 0x65, 0x74, 0x61, 0x6A, 0x55, 0x52, 0x7A, 0xC4, 0x6A,
    0x51, 0xC3, 0x6A, 0x52, 0xC3, 0x02, 0x6E, 0x6F, 0xC3, 0x6A, 0x55, 0xC3,
    0x51, 0x93, 0x52, 0x72, 0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74,
    0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x50, 0x75, 0x74, 0x61, 0x61, 0x6A,
    0x51, 0xC3, 0x6A, 0x52, 0xC3, 0x04, 0x61, 0x64, 0x64, 0x72, 0xC3, 0x6A,
    0x00, 0xC3, 0x51, 0xC3, 0x7E, 0x52, 0x52, 0x72, 0x68, 0x0F, 0x4E, 0x65,
    0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x50, 0x75,
    0x74, 0x61, 0x6A, 0x00, 0xC3, 0x51, 0xC3, 0x6A, 0x00, 0xC3, 0x52, 0xC3,
    0x7C, 0x04, 0x76, 0x6F, 0x74, 0x65, 0x53, 0xC1, 0x68, 0x12, 0x4E, 0x65,
    0x6F, 0x2E, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65, 0x2E, 0x4E, 0x6F,
    0x74, 0x69, 0x66, 0x79, 0x51, 0x6C, 0x75, 0x66, 0x01, 0x17, 0xC5, 0x6B,
    0x6A, 0x00, 0x52, 0x7A, 0xC4, 0x68, 0x16, 0x4E, 0x65, 0x6F, 0x2E, 0x53,
    0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x47, 0x65, 0x74, 0x43, 0x6F,
    0x6E, 0x74, 0x65, 0x78, 0x74, 0x61, 0x6A, 0x51, 0x52, 0x7A, 0xC4, 0x6A,
    0x00, 0xC3, 0xC0, 0x54, 0x9F, 0x64, 0x07, 0x00, 0x00, 0x6C, 0x75, 0x66,
    0x61, 0x6A, 0x00, 0xC3, 0x00, 0xC3, 0x65, 0x7E, 0x01, 0x6A, 0x52, 0x52,
    0x7A, 0xC4, 0x6A, 0x51, 0xC3, 0x6A, 0x52, 0xC3, 0x02, 0x69, 0x64, 0xC3,
    0x7C, 0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61,
    0x67, 0x65, 0x2E, 0x47, 0x65, 0x74, 0x61, 0x64, 0x2B, 0x00, 0x12, 0x41,
    0x6C, 0x72, 0x65, 0x61, 0x64, 0x79, 0x20, 0x61, 0x20, 0x50, 0x72, 0x6F,
    0x70, 0x6F, 0x73, 0x61, 0x6C, 0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x52,
    0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65, 0x2E, 0x4C, 0x6F, 0x67, 0x00, 0x6C,
    0x75, 0x66, 0x61, 0x6A, 0x51, 0xC3, 0x6A, 0x52, 0xC3, 0x02, 0x69, 0x64,
    0xC3, 0x51, 0x52, 0x72, 0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74,
    0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x50, 0x75, 0x74, 0x61, 0x6A, 0x51,
    0xC3, 0x6A, 0x52, 0xC3, 0x04, 0x74, 0x65, 0x78, 0x74, 0xC3, 0x6A, 0x00,
    0xC3, 0x51, 0xC3, 0x52, 0x72, 0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x53,
    0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x50, 0x75, 0x74, 0x61, 0x6A,
    0x51, 0xC3, 0x6A, 0x52, 0xC3, 0x03, 0x79, 0x65, 0x73, 0xC3, 0x00, 0x52,
    0x72, 0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61,
    0x67, 0x65, 0x2E, 0x50, 0x75, 0x74, 0x61, 0x6A, 0x51, 0xC3, 0x6A, 0x52,
    0xC3, 0x02, 0x6E, 0x6F, 0xC3, 0x00, 0x52, 0x72, 0x68, 0x0F, 0x4E, 0x65,
    0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x50, 0x75,
    0x74, 0x61, 0x52, 0x6A, 0x53, 0x52, 0x7A, 0xC4, 0x61, 0x61, 0x6A, 0x53,
    0xC3, 0x6A, 0x00, 0xC3, 0xC0, 0x9F, 0x64, 0x60, 0x00, 0x6A, 0x00, 0xC3,
    0x6A, 0x53, 0xC3, 0xC3, 0x6A, 0x54, 0x52, 0x7A, 0xC4, 0x6A, 0x54, 0xC3,
    0xC0, 0x01, 0x14, 0x9E, 0x64, 0x17, 0x00, 0x12, 0x62, 0x61, 0x64, 0x20,
    0x61, 0x64, 0x64, 0x72, 0x65, 0x73, 0x73, 0x20, 0x66, 0x6F, 0x72, 0x6D,
    0x61, 0x74, 0xF0, 0x61, 0x6A, 0x51, 0xC3, 0x6A, 0x52, 0xC3, 0x04, 0x61,
    0x64, 0x64, 0x72, 0xC3, 0x6A, 0x54, 0xC3, 0x7E, 0x51, 0x52, 0x72, 0x68,
    0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65,
    0x2E, 0x50, 0x75, 0x74, 0x61, 0x6A, 0x53, 0xC3, 0x51, 0x93, 0x6A, 0x53,
    0x52, 0x7A, 0xC4, 0x62, 0x9A, 0xFF, 0x61, 0x61, 0x61, 0x6A, 0x00, 0xC3,
    0x00, 0xC3, 0x6A, 0x00, 0xC3, 0x51, 0xC3, 0x7C, 0x11, 0x72, 0x65, 0x67,
    0x69, 0x73, 0x74, 0x65, 0x72, 0x5F, 0x70, 0x72, 0x6F, 0x70, 0x6F, 0x73,
    0x61, 0x6C, 0x53, 0xC1, 0x68, 0x12, 0x4E, 0x65, 0x6F, 0x2E, 0x52, 0x75,
    0x6E, 0x74, 0x69, 0x6D, 0x65, 0x2E, 0x4E, 0x6F, 0x74, 0x69, 0x66, 0x79,
    0x51, 0x6C, 0x75, 0x66, 0x56, 0xC5, 0x6B, 0x6A, 0x00, 0x52, 0x7A, 0xC4,
    0x02, 0x49, 0x44, 0x6A, 0x00, 0xC3, 0x7E, 0x6A, 0x51, 0x52, 0x7A, 0xC4,
    0xC7, 0x6A, 0x52, 0x52, 0x7A, 0xC4, 0x6A, 0x51, 0xC3, 0x6A, 0x52, 0xC3,
    0x02, 0x69, 0x64, 0x7B, 0xC4, 0x01, 0x54, 0x6A, 0x51, 0xC3, 0x7E, 0x6A,
    0x52, 0xC3, 0x04, 0x74, 0x65, 0x78, 0x74, 0x7B, 0xC4, 0x01, 0x59, 0x6A,
    0x51, 0xC3, 0x7E, 0x6A, 0x52, 0xC3, 0x03, 0x79, 0x65, 0x73, 0x7B, 0xC4,
    0x01, 0x4E, 0x6A, 0x51, 0xC3, 0x7E, 0x6A, 0x52, 0xC3, 0x02, 0x6E, 0x6F,
    0x7B, 0xC4, 0x01, 0x41, 0x6A, 0x51, 0xC3, 0x7E, 0x6A, 0x52, 0xC3, 0x04,
    0x61, 0x64, 0x64, 0x72, 0x7B, 0xC4, 0x6A, 0x52, 0xC3, 0x6C, 0x75, 0x66,
    0x5E, 0xC5, 0x6B, 0x6A, 0x00, 0x52, 0x7A, 0xC4, 0x6A, 0x51, 0x52, 0x7A,
    0xC4, 0x6A, 0x51, 0xC3, 0x6A, 0x00, 0xC3, 0x94, 0x6A, 0x52, 0x52, 0x7A,
    0xC4, 0x6A, 0x52, 0xC3, 0xC5, 0x6A, 0x53, 0x52, 0x7A, 0xC4, 0x00, 0x6A,
    0x54, 0x52, 0x7A, 0xC4, 0x6A, 0x00, 0xC3, 0x6A, 0x55, 0x52, 0x7A, 0xC4,
    0x61, 0x61, 0x6A, 0x00, 0xC3, 0x6A, 0x51, 0xC3, 0x9F, 0x64, 0x33, 0x00,
    0x6A, 0x54, 0xC3, 0x6A, 0x55, 0xC3, 0x93, 0x6A, 0x56, 0x52, 0x7A, 0xC4,
    0x6A, 0x56, 0xC3, 0x6A, 0x53, 0xC3, 0x6A, 0x54, 0xC3, 0x7B, 0xC4, 0x6A,
    0x54, 0xC3, 0x51, 0x93, 0x6A, 0x54, 0x52, 0x7A, 0xC4, 0x6A, 0x55, 0xC3,
    0x6A, 0x54, 0xC3, 0x93, 0x6A, 0x00, 0x52, 0x7A, 0xC4, 0x62, 0xC8, 0xFF,
    0x61, 0x61, 0x61, 0x6A, 0x53, 0xC3, 0x6C, 0x75, 0x66
};

        /// <summary>
        /// https://github.com/belane/SmartVote/blob/master/smartvote.py
        /// </summary>
        [TestMethod]
        public void SmartVote()
        {
            const string VoteId = "q01";

            // Create arguments

            ExecutionEngineArgs args = new ExecutionEngineArgs()
            {
                InteropService = new DummyInteropService(),
                ScriptTable = new DummyScriptTable(),
                Trigger = ETriggerType.Application,
                MessageProvider = new DummyMessageProvider(),
                Logger = new ExecutionEngineLogger(ELogVerbosity.None)
            };

            args.Logger.OnStepInto += (context) =>
            { Console.WriteLine(context.ToString()); };

            args.Logger.OnAltStackChange += (stack, item, index, oper) =>
            { Console.WriteLine("AltStack: " + index.ToString() + "-" + oper.ToString()); };

            args.Logger.OnEvaluationStackChange += (stack, item, index, oper) =>
            { Console.WriteLine("EvStack: " + index.ToString() + "-" + oper.ToString()); };

            args.Logger.OnExecutionContextChange += (stack, item, index, oper) =>
            { Console.WriteLine("ExeStack: " + index.ToString() + "-" + oper.ToString()); };

            args.InteropService.OnLog += (sender, e) => { Console.WriteLine("Log: " + e.Message); };
            args.InteropService.OnNotify += (sender, e) => { Console.WriteLine("Notification: " + e.State.ToString()); };

            using (ScriptBuilder arguments = new ScriptBuilder())
            using (ExecutionEngine engine = NeoVM.CreateEngine(args))
            // for (int x = 0; x < 5000; x++) // Benchmark
            {
                // Register proposal

                arguments.Clear();
                arguments.EmitMainPush("register_proposal", new object[] { VoteId, "My proposal", new byte[20], new byte[20] });

                engine.Clean(0);
                engine.LoadScript(script);
                engine.LoadScript(arguments);

                // Execute

                Assert.AreEqual(engine.Execute(), EVMState.HALT);
                Assert.AreEqual(engine.EvaluationStack.Pop<IntegerStackItem>().Value, 0x01);
                CheckClean(engine);

                // Vote

                arguments.Clear();
                arguments.EmitMainPush("vote", new object[] { VoteId, new byte[20], 1 });

                engine.Clean(1);
                engine.LoadScript(script);
                engine.LoadScript(arguments);

                // Execute

                Assert.AreEqual(engine.Execute(), EVMState.HALT);
                Assert.AreEqual(engine.EvaluationStack.Pop<IntegerStackItem>().Value, 0x01);
                CheckClean(engine);

                // Count

                arguments.Clear();
                arguments.EmitMainPush("count", new object[] { VoteId });

                engine.Clean(2);
                engine.LoadScript(script);
                engine.LoadScript(arguments);

                // Execute

                Assert.AreEqual(engine.Execute(), EVMState.HALT);
                Assert.AreEqual(engine.EvaluationStack.Pop<ByteArrayStackItem>().Value.Length, 0x00);
                CheckClean(engine);
            }
        }
    }
}