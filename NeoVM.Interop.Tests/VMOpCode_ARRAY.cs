﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoVM.Interop.Enums;
using NeoVM.Interop.Types;
using NeoVM.Interop.Types.StackItems;

namespace NeoVM.Interop.Tests
{
    [TestClass]
    public class VMOpCode_ARRAY : VMOpCodeTest
    {
        [TestMethod]
        public void ARRAYSIZE()
        {
            // With wrong type

            using (ScriptBuilder script = new ScriptBuilder())
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                script.EmitSysCall("System.ExecutionEngine.GetScriptContainer");
                script.Emit(EVMOpCode.ARRAYSIZE);
                script.Emit(EVMOpCode.RET);

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.FAULT, engine.Execute());

                // Check

                CheckClean(engine, false);
            }

            // Without push

            using (ScriptBuilder script = new ScriptBuilder(EVMOpCode.ARRAYSIZE))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.FAULT, engine.Execute());

                // Check

                CheckClean(engine, false);
            }

            // Real test

            using (ScriptBuilder script = new ScriptBuilder
                (
                    EVMOpCode.PUSH3,
                    EVMOpCode.NEWARRAY,
                    EVMOpCode.ARRAYSIZE,

                    EVMOpCode.PUSHBYTES1, 0x00,
                    EVMOpCode.ARRAYSIZE,

                    EVMOpCode.RET
                ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.HALT, engine.Execute());

                // Check

                Assert.AreEqual(engine.EvaluationStack.Pop<IntegerStackItem>().Value, 1);
                Assert.AreEqual(engine.EvaluationStack.Pop<IntegerStackItem>().Value, 3);

                CheckClean(engine);
            }
        }

        [TestMethod]
        public void PACK()
        {
            // Overflow

            using (ScriptBuilder script = new ScriptBuilder
                (
                EVMOpCode.PUSH5,
                EVMOpCode.PUSH2,
                EVMOpCode.PACK
                ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.FAULT, engine.Execute());

                // Check

                Assert.AreEqual(engine.EvaluationStack.Pop<IntegerStackItem>().Value, 5);

                CheckClean(engine, false);
            }

            // Wrong type

            using (ScriptBuilder script = new ScriptBuilder
                (
                EVMOpCode.NEWMAP,
                EVMOpCode.PACK
                ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.FAULT, engine.Execute());

                // Check

                CheckClean(engine, false);
            }

            // Without push

            using (ScriptBuilder script = new ScriptBuilder(EVMOpCode.PACK))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.FAULT, engine.Execute());

                // Check

                CheckClean(engine, false);
            }

            // Real test

            using (ScriptBuilder script = new ScriptBuilder
                (
                    EVMOpCode.PUSH5,
                    EVMOpCode.PUSH6,
                    EVMOpCode.PUSH2,
                    EVMOpCode.PACK,
                    EVMOpCode.RET
                ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.HALT, engine.Execute());

                // Check

                CheckArrayPop(engine.EvaluationStack, false, 0x06, 0x05);

                CheckClean(engine);
            }
        }

        [TestMethod]
        public void UNPACK()
        {
            // Without array

            using (ScriptBuilder script = new ScriptBuilder
                   (
                       EVMOpCode.PUSH10,
                       EVMOpCode.UNPACK,
                       EVMOpCode.RET
                   ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.FAULT, engine.Execute());

                // Check

                CheckClean(engine, false);
            }

            // Without push

            using (ScriptBuilder script = new ScriptBuilder
                   (
                       EVMOpCode.UNPACK,
                       EVMOpCode.RET
                   ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.FAULT, engine.Execute());

                // Check

                CheckClean(engine, false);
            }

            // Real tests

            using (ScriptBuilder script = new ScriptBuilder
                   (
                       EVMOpCode.PUSH5,
                       EVMOpCode.PUSH6,
                       EVMOpCode.PUSH2,
                       EVMOpCode.PACK,
                       EVMOpCode.UNPACK,
                       EVMOpCode.RET
                   ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.HALT, engine.Execute());

                // Check

                Assert.IsTrue(engine.EvaluationStack.Pop<IntegerStackItem>().Value == 0x02);
                Assert.IsTrue(engine.EvaluationStack.Pop<IntegerStackItem>().Value == 0x06);
                Assert.IsTrue(engine.EvaluationStack.Pop<IntegerStackItem>().Value == 0x05);

                CheckClean(engine);
            }
        }

        public void PICKITEM_ARRAY_STRUCT(bool isStruct)
        {
            // Wrong array

            using (ScriptBuilder script = new ScriptBuilder
                   (
                       EVMOpCode.PUSH0,
                       EVMOpCode.PUSH0,
                       EVMOpCode.PICKITEM
                   ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.FAULT, engine.Execute());

                // Check

                CheckClean(engine, false);
            }

            // Without push

            using (ScriptBuilder script = new ScriptBuilder
                   (
                       EVMOpCode.PICKITEM
                   ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.FAULT, engine.Execute());

                // Check

                CheckClean(engine, false);
            }

            // Wrong key type

            using (ScriptBuilder script = new ScriptBuilder
                   (
                       EVMOpCode.PUSH6,
                       EVMOpCode.NEWMAP,
                       EVMOpCode.PICKITEM,
                       EVMOpCode.RET
                   ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.FAULT, engine.Execute());

                // Check

                Assert.AreEqual(engine.EvaluationStack.Pop<IntegerStackItem>().Value, 6);

                CheckClean(engine, false);
            }

            // Out of bounds

            using (ScriptBuilder script = new ScriptBuilder
                   (
                       EVMOpCode.PUSH2,
                       isStruct ? EVMOpCode.NEWSTRUCT : EVMOpCode.NEWARRAY,
                       EVMOpCode.PUSH3,
                       EVMOpCode.PICKITEM,
                       EVMOpCode.RET
                   ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.FAULT, engine.Execute());

                // Check

                CheckClean(engine, false);
            }

            // Real test

            using (ScriptBuilder script = new ScriptBuilder
                   (
                       // Create array or struct

                       EVMOpCode.PUSH3,
                       isStruct ? EVMOpCode.NEWSTRUCT : EVMOpCode.NEWARRAY,

                       // Make a copy

                       EVMOpCode.TOALTSTACK,

                       // [0]=1

                       EVMOpCode.DUPFROMALTSTACK,
                       EVMOpCode.PUSH0,
                       EVMOpCode.PUSH1,
                       EVMOpCode.SETITEM,

                       // [1]=2

                       EVMOpCode.DUPFROMALTSTACK,
                       EVMOpCode.PUSH1,
                       EVMOpCode.PUSH2,
                       EVMOpCode.SETITEM,

                       // [2]=3

                       EVMOpCode.DUPFROMALTSTACK,
                       EVMOpCode.PUSH2,
                       EVMOpCode.PUSH3,
                       EVMOpCode.SETITEM,

                       // Pick

                       EVMOpCode.FROMALTSTACK,
                       EVMOpCode.PUSH2,
                       EVMOpCode.PICKITEM,
                       EVMOpCode.RET
                   ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.HALT, engine.Execute());

                // Check

                Assert.AreEqual(engine.EvaluationStack.Pop<IntegerStackItem>().Value, 3);

                CheckClean(engine);
            }
        }

        [TestMethod]
        public void PICKITEM_ARRAY()
        {
            PICKITEM_ARRAY_STRUCT(false);
        }

        [TestMethod]
        public void PICKITEM_STRUCT()
        {
            PICKITEM_ARRAY_STRUCT(true);
        }

        public void SETITEM_ARRAY_STRUCT(bool isStruct)
        {
            // Map in key

            using (ScriptBuilder script = new ScriptBuilder
                   (
                       EVMOpCode.PUSH1,
                       EVMOpCode.NEWMAP,
                       EVMOpCode.PUSH0,
                       EVMOpCode.SETITEM
                   ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.FAULT, engine.Execute());

                // Check

                Assert.AreEqual(engine.EvaluationStack.Pop<IntegerStackItem>().Value, 1);

                CheckClean(engine, false);
            }

            // Without array

            using (ScriptBuilder script = new ScriptBuilder
                   (
                       EVMOpCode.PUSH0,
                       EVMOpCode.PUSH0,
                       EVMOpCode.PUSH0,
                       EVMOpCode.SETITEM
                   ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.FAULT, engine.Execute());

                // Check

                CheckClean(engine, false);
            }

            // Without push

            using (ScriptBuilder script = new ScriptBuilder
                   (
                       EVMOpCode.SETITEM
                   ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.FAULT, engine.Execute());

                // Check

                CheckClean(engine, false);
            }

            // Out of bounds

            using (ScriptBuilder script = new ScriptBuilder
                   (
                       EVMOpCode.PUSH1,
                       isStruct ? EVMOpCode.NEWSTRUCT : EVMOpCode.NEWARRAY,
                       EVMOpCode.PUSH1,
                       EVMOpCode.PUSH5,
                       EVMOpCode.SETITEM,
                       EVMOpCode.RET
                   ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.FAULT, engine.Execute());

                // Check

                CheckClean(engine, false);
            }

            // Clone test (1)

            using (ScriptBuilder script = new ScriptBuilder
                   (
                       // Create new array

                       EVMOpCode.PUSH1,
                       isStruct ? EVMOpCode.NEWSTRUCT : EVMOpCode.NEWARRAY,
                       EVMOpCode.DUP,

                       // Init [0]=0x05

                       EVMOpCode.PUSH0,
                       EVMOpCode.PUSH5,
                       EVMOpCode.SETITEM,

                       // Clone

                       EVMOpCode.TOALTSTACK,
                       EVMOpCode.DUPFROMALTSTACK,
                       EVMOpCode.DUP,

                       // Set [0]=0x04

                       EVMOpCode.PUSH0,
                       EVMOpCode.PUSH4,
                       EVMOpCode.SETITEM,
                       EVMOpCode.RET
                   ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                engine.StepInto(3);
                Assert.AreEqual(engine.AltStack.Count, 0);
                Assert.AreEqual(engine.EvaluationStack.Count, 2);

                engine.StepInto(3);
                Assert.AreEqual(engine.AltStack.Count, 0);
                Assert.AreEqual(engine.EvaluationStack.Count, 1);

                CheckArrayPeek(engine.EvaluationStack, 0, isStruct, 0x05);

                engine.StepInto(3);
                Assert.AreEqual(engine.AltStack.Count, 1);
                Assert.AreEqual(engine.EvaluationStack.Count, 2);

                engine.StepInto(4);
                Assert.AreEqual(engine.AltStack.Count, 1);
                Assert.AreEqual(engine.EvaluationStack.Count, 1);

                Assert.AreEqual(EVMState.HALT, engine.State);

                // Check

                CheckArrayPop(engine.EvaluationStack, isStruct, 0x04);
                CheckArrayPop(engine.AltStack, isStruct, 0x04);

                CheckClean(engine);
            }

            /*
            // Clone test (2)

            using (ScriptBuilder script = new ScriptBuilder
                   (
                       // Create new array

                       EVMOpCode.PUSH1,
                       isStruct ? EVMOpCode.NEWSTRUCT : EVMOpCode.NEWARRAY,
                       EVMOpCode.DUP,

                       // Init [0]=0x05
                       EVMOpCode.PUSH0,
                       EVMOpCode.PUSH5,
                       EVMOpCode.SETITEM,
                       EVMOpCode.TOALTSTACK,

                       // Create new array

                       EVMOpCode.PUSH1,
                       isStruct ? EVMOpCode.NEWSTRUCT : EVMOpCode.NEWARRAY,
                       EVMOpCode.DUP,

                       // Init [0]=0x04
                       EVMOpCode.PUSH0,
                       EVMOpCode.PUSH4,
                       EVMOpCode.SETITEM,
                       EVMOpCode.DUPFROMALTSTACK,
                       EVMOpCode.SWAP,
                       EVMOpCode.DUP,
                       EVMOpCode.TOALTSTACK,

                       // Set [0]=0x04

                       // Clone both in both stacks

                       EVMOpCode.TOALTSTACK,
                       EVMOpCode.PUSH0,
                       EVMOpCode.FROMALTSTACK,

                       EVMOpCode.SETITEM,
                       EVMOpCode.RET
                   ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                engine.StepInto(3);
                Assert.AreEqual(engine.AltStack.Count, 0);
                Assert.AreEqual(engine.EvaluationStack.Count, 2);

                engine.StepInto(4);
                Assert.AreEqual(engine.AltStack.Count, 1);
                Assert.AreEqual(engine.EvaluationStack.Count, 0);

                CheckArrayPeek(engine.AltStack, 0, isStruct, 0x05);

                engine.StepInto(3);
                Assert.AreEqual(engine.AltStack.Count, 1);
                Assert.AreEqual(engine.EvaluationStack.Count, 2);

                CheckArrayPeek(engine.AltStack, 0, isStruct, 0x05);

                engine.StepInto(7);
                Assert.AreEqual(engine.AltStack.Count, 2);
                Assert.AreEqual(engine.EvaluationStack.Count, 2);

                CheckArrayPeek(engine.AltStack, 0, isStruct, 0x04);
                CheckArrayPeek(engine.AltStack, 1, isStruct, 0x05);

                CheckArrayPeek(engine.EvaluationStack, 0, isStruct, 0x04);
                CheckArrayPeek(engine.EvaluationStack, 1, isStruct, 0x05);

                engine.StepInto(5);
                Assert.AreEqual(engine.AltStack.Count, 2);
                Assert.AreEqual(engine.EvaluationStack.Count, 0);

                Assert.AreEqual(EVMState.HALT, engine.State);

                CheckArrayPop(engine.AltStack, isStruct, 0x04);

                if (isStruct)
                {
                    CheckArrayPop(engine.AltStack, isStruct, 0x05);
                }
                else
                {
                    using (ArrayStackItem arr = engine.AltStack.Pop<ArrayStackItem>())
                    {
                        Assert.IsTrue(arr != null);
                        Assert.AreEqual(isStruct, arr.IsStruct);
                        Assert.IsTrue(arr.Count == 1);

                        Assert.IsTrue(arr[0] is ArrayStackItem);
                        ArrayStackItem i = arr[0] as ArrayStackItem;
                        Assert.IsTrue(i.Count == 1);

                        IntegerStackItem ii = i[0] as IntegerStackItem;
                        Assert.AreEqual(ii.Value, 0x04);
                    }
                }

                // Check

                CheckClean(engine);
            }
            */

            // Real test

            using (ScriptBuilder script = new ScriptBuilder
                   (
                       EVMOpCode.PUSH1,
                       isStruct ? EVMOpCode.NEWSTRUCT : EVMOpCode.NEWARRAY,
                       EVMOpCode.DUP,
                       EVMOpCode.PUSH0,
                       EVMOpCode.PUSH5,
                       EVMOpCode.SETITEM,
                       EVMOpCode.RET
                   ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.HALT, engine.Execute());

                // Check

                Assert.IsTrue(engine.EvaluationStack.Count == 1);

                CheckArrayPop(engine.EvaluationStack, isStruct, 0x05);

                CheckClean(engine);
            }
        }

        [TestMethod]
        public void SETITEM_ARRAY()
        {
            SETITEM_ARRAY_STRUCT(false);
        }

        [TestMethod]
        public void SETITEM_STRUCT()
        {
            SETITEM_ARRAY_STRUCT(true);
        }

        void NEWARRAY_NEWSTRUCT(bool isStruct)
        {
            // Without push

            using (ScriptBuilder script = new ScriptBuilder
            (
                isStruct ? EVMOpCode.NEWSTRUCT : EVMOpCode.NEWARRAY,
                EVMOpCode.RET
            ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.FAULT, engine.Execute());

                // Check

                CheckClean(engine, false);
            }

            // With push (-1)

            using (ScriptBuilder script = new ScriptBuilder
            (
                EVMOpCode.PUSHM1,
                isStruct ? EVMOpCode.NEWSTRUCT : EVMOpCode.NEWARRAY,
                EVMOpCode.RET
            ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.FAULT, engine.Execute());

                // Check

                CheckClean(engine, false);
            }

            // Real test

            using (ScriptBuilder script = new ScriptBuilder
            (
                EVMOpCode.PUSH2,
                isStruct ? EVMOpCode.NEWSTRUCT : EVMOpCode.NEWARRAY,
                EVMOpCode.RET
            ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.HALT, engine.Execute());

                // Check

                CheckArrayPop(engine.EvaluationStack, isStruct, false, false);

                CheckClean(engine);
            }
        }

        [TestMethod]
        public void NEWARRAY() { NEWARRAY_NEWSTRUCT(false); }

        [TestMethod]
        public void NEWSTRUCT() { NEWARRAY_NEWSTRUCT(true); }

        [TestMethod]
        public void NEWMAP()
        {
            using (ScriptBuilder script = new ScriptBuilder
                   (
                       EVMOpCode.NEWMAP,
                       EVMOpCode.RET
                   ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.HALT, engine.Execute());

                // Check

                Assert.IsTrue(engine.EvaluationStack.Pop() is MapStackItem);

                CheckClean(engine);
            }
        }

        [TestMethod]
        public void APPEND()
        {
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void REVERSE()
        {
            // Without push

            using (ScriptBuilder script = new ScriptBuilder(EVMOpCode.REVERSE))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.FAULT, engine.Execute());

                // Check

                CheckClean(engine, false);
            }

            // Without Array

            using (ScriptBuilder script = new ScriptBuilder
                    (
                        EVMOpCode.PUSH9,
                        EVMOpCode.REVERSE
                    ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.FAULT, engine.Execute());

                // Check

                CheckClean(engine, false);
            }

            // Real test

            using (ScriptBuilder script = new ScriptBuilder
                    (
                        EVMOpCode.PUSH9,
                        EVMOpCode.PUSH8,
                        EVMOpCode.PUSH2,
                        EVMOpCode.PACK,
                        EVMOpCode.DUP,
                        EVMOpCode.REVERSE
                    ))
            using (ExecutionEngine engine = NeoVM.CreateEngine(Args))
            {
                // Load script

                engine.LoadScript(script);

                // Execute

                Assert.AreEqual(EVMState.HALT, engine.Execute());

                // Check

                CheckArrayPop(engine.EvaluationStack, false, 0x09, 0x08);

                CheckClean(engine);
            }
        }

        [TestMethod]
        public void REMOVE()
        {
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void HASKEY()
        {
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void KEYS()
        {
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void VALUES()
        {
            Assert.IsFalse(true);
        }
    }
}