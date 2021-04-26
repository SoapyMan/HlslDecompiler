﻿using System.IO;
using System.Text;

namespace HlslDecompiler.DirectXShaderModel
{
    public class ShaderReader : BinaryReader
    {
        public ShaderReader(Stream input, bool leaveOpen = false)
            : base(input, new UTF8Encoding(false, true), leaveOpen)
        {
        }

        virtual public ShaderModel ReadShader()
        {
            // Version token
            byte minorVersion = ReadByte();
            byte majorVersion = ReadByte();
            ShaderType shaderType = (ShaderType)ReadUInt16();

            var shader = new ShaderModel(majorVersion, minorVersion, shaderType);

            while (true)
            {
                Instruction instruction = ReadInstruction();
                InstructionVerifier.Verify(instruction);
                shader.Instructions.Add(instruction);
                if (instruction.Opcode == Opcode.End) break;
            }

            return shader;
        }

        private Instruction ReadInstruction()
        {
            uint instructionToken = ReadUInt32();
            Opcode opcode = (Opcode)(instructionToken & 0xffff);

            int size;
            if (opcode == Opcode.Comment)
            {
                size = (int)((instructionToken >> 16) & 0x7FFF);
            }
            else
            {
                size = (int)((instructionToken >> 24) & 0x0f);
            }

            uint[] paramTokens = new uint[size];
            for (int i = 0; i < size; i++)
            {
                paramTokens[i] = ReadUInt32();
            }
            var instruction = new Instruction(opcode, paramTokens);

            if (opcode != Opcode.Comment)
            {
                instruction.Modifier = (int)((instructionToken >> 16) & 0xff);
                instruction.Predicated = (instructionToken & 0x10000000) != 0;
                System.Diagnostics.Debug.Assert((instructionToken & 0xE0000000) == 0);
            }

            return instruction;
        }
    }
}
