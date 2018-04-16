﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoVM.Interop.Enums;
using NeoVM.Interop.Types;
using NeoVM.Interop.Types.StackItems;
using System.Linq;

namespace NeoVM.Interop.Tests
{
    [TestClass]
    public class VMOpCode_PUSHES : VMOpCodeTest
    {
        [TestMethod]
        public void PUSH0()
        {
            using (ScriptBuilder script = new ScriptBuilder
            (
                EVMOpCode.PUSH0,
                EVMOpCode.RET
            ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.HALT, engine.Execute());

                // Check

                Assert.IsTrue(engine.EvaluationStack.Pop<ByteArrayStackItem>().Value.Length == 0);

                CheckClean(engine);
            }
        }

        [TestMethod]
        public void PUSHBYTES1_TO_PUSHBYTES75()
        {
            for (int x = 0; x < 75; x++)
                using (ScriptBuilder script = new ScriptBuilder())
                {
                    // Generate Script

                    byte[] data = new byte[((byte)EVMOpCode.PUSHBYTES1 + x)];
                    for (byte y = 0; y < data.Length; y++) data[y] = y;

                    script.Emit((byte)((byte)EVMOpCode.PUSHBYTES1 + x));
                    script.Emit(data, 0, data.Length);

                    // Execute

                    using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
                    {
                        // Load script

                        engine.LoadScript(script.ToArray());

                        // Execute

                        Assert.AreEqual(EVMState.HALT, engine.Execute());

                        // Check

                        Assert.AreEqual(1, engine.EvaluationStack.Count);

                        Assert.IsTrue(engine.EvaluationStack.Pop<ByteArrayStackItem>().Value.SequenceEqual(data));

                        CheckClean(engine);
                    }

                    // Try error

                    using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
                    {
                        // Load wrong Script

                        engine.LoadScript(script.ToArray().Take((int)script.Length - 1).ToArray());

                        // Execute

                        Assert.AreEqual(EVMState.FAULT, engine.Execute());

                        // Check

                        CheckClean(engine, false);
                    }
                }
        }

        [TestMethod]
        public void PUSHDATA1()
        {
            using (ScriptBuilder script = new ScriptBuilder
            (
                EVMOpCode.PUSHDATA1, new byte[]
                {
                    0x04,
                    0x01, 0x02, 0x03, 0x04
                }
            ))
            {
                using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
                {
                    // Load Script

                    engine.LoadScript(script);

                    // Execute

                    Assert.AreEqual(EVMState.HALT, engine.Execute());

                    // Check

                    Assert.IsTrue(engine.EvaluationStack.Pop<ByteArrayStackItem>().Value.SequenceEqual(new byte[]
                    {
                    0x01,0x02,0x03,0x04
                    }));

                    CheckClean(engine);
                }

                // Try error

                using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
                {
                    // Load wrong Script

                    byte[] badScript = script.ToArray();
                    badScript[1]++;
                    engine.LoadScript(badScript);

                    // Execute

                    Assert.AreEqual(EVMState.FAULT, engine.Execute());

                    // Check

                    CheckClean(engine, false);
                }
            }
        }

        [TestMethod]
        public void PUSHDATA2()
        {
            using (ScriptBuilder script = new ScriptBuilder
            (
                 EVMOpCode.PUSHDATA2, new byte[]
                 {
                    0x04, 0x00,
                    0x01, 0x02, 0x03, 0x04
             }))
            {
                using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
                {
                    // Load Script

                    engine.LoadScript(script);

                    // Execute

                    Assert.AreEqual(EVMState.HALT, engine.Execute());

                    // Check

                    Assert.IsTrue(engine.EvaluationStack.Pop<ByteArrayStackItem>().Value.SequenceEqual(new byte[]
                    {
                    0x01,0x02,0x03,0x04
                    }));

                    CheckClean(engine);
                }

                // Try error

                using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
                {
                    // Load wrong Script

                    byte[] badScript = script.ToArray();
                    badScript[1]++;
                    engine.LoadScript(badScript);

                    // Execute

                    Assert.AreEqual(EVMState.FAULT, engine.Execute());

                    // Check

                    CheckClean(engine, false);
                }
            }
        }

        [TestMethod]
        public void PUSHDATA4()
        {
            using (ScriptBuilder script = new ScriptBuilder
            (
                EVMOpCode.PUSHDATA4, new byte[]
                {
                0x04, 0x00, 0x00, 0x00,
                0x01, 0x02, 0x03, 0x04
                }
            ))
            {
                using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
                {
                    // Load Script

                    engine.LoadScript(script);

                    // Execute

                    Assert.AreEqual(EVMState.HALT, engine.Execute());

                    // Check

                    Assert.IsTrue(engine.EvaluationStack.Pop<ByteArrayStackItem>().Value.SequenceEqual(new byte[]
                    {
                    0x01,0x02,0x03,0x04
                    }));

                    CheckClean(engine);
                }

                // Try error

                using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
                {
                    // Load wrong Script

                    byte[] badScript = script.ToArray();
                    badScript[1]++;
                    engine.LoadScript(badScript);

                    // Execute

                    Assert.AreEqual(EVMState.FAULT, engine.Execute());

                    // Check

                    CheckClean(engine, false);
                }
            }
        }

        [TestMethod]
        public void PUSHM1()
        {
            using (ScriptBuilder script = new ScriptBuilder
            (
                EVMOpCode.PUSHM1,
                EVMOpCode.RET
            ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.HALT, engine.Execute());

                // Check

                Assert.AreEqual(engine.EvaluationStack.Pop<IntegerStackItem>().Value, -1);

                CheckClean(engine);
            }
        }

        [TestMethod]
        public void PUSH1_TO_PUSH16()
        {
            using (ScriptBuilder script = new ScriptBuilder
            (
                EVMOpCode.PUSH1, EVMOpCode.PUSH2,
                EVMOpCode.PUSH3, EVMOpCode.PUSH4,
                EVMOpCode.PUSH5, EVMOpCode.PUSH6,
                EVMOpCode.PUSH7, EVMOpCode.PUSH8,
                EVMOpCode.PUSH9, EVMOpCode.PUSH10,
                EVMOpCode.PUSH11, EVMOpCode.PUSH12,
                EVMOpCode.PUSH13, EVMOpCode.PUSH14,
                EVMOpCode.PUSH15, EVMOpCode.PUSH16,
                EVMOpCode.RET
            ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load Script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.HALT, engine.Execute());

                // Check

                for (int x = 16; x >= 1; x--)
                {
                    Assert.AreEqual(x, engine.EvaluationStack.Count);
                    Assert.AreEqual(engine.EvaluationStack.Pop<IntegerStackItem>().Value, x);
                }

                CheckClean(engine);
            }
        }
    }
}