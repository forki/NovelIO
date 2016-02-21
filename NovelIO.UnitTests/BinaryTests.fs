﻿(*
   Copyright 2015 Philip Curzon

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*)

namespace NovelFS.NovelIO.UnitTests

open NovelFS.NovelIO
open NovelFS.NovelIO.BinaryParser
open FsCheck
open FsCheck.Xunit

type ``Binary Parser Tests`` =
    [<Property>]
    static member ``Parse byte from array of one byte`` (byte : byte) =
        let bytes = [|byte|]
        let bParser = BinaryParser.parseByte
        let result =
            match BinaryParser.run bytes bParser with
            |ParseSuccess byte -> byte
            |ParseFailure err -> failwith "failed"
        result = byte
    [<Property>]
    static member ``Parse int16 from array of bytes`` (i16 : int16) =
        let bytes = System.BitConverter.GetBytes i16
        let bParser = BinaryParser.parseInt16
        let result =
            match BinaryParser.run bytes bParser with
            |ParseSuccess byte -> byte
            |ParseFailure err -> failwith "failed"
        result = i16
    [<Property>]
    static member ``Parse int32 from array of bytes`` (i32 : int32) =
        let bytes = System.BitConverter.GetBytes i32
        let bParser = BinaryParser.parseInt32
        let result =
            match BinaryParser.run bytes bParser with
            |ParseSuccess byte -> byte
            |ParseFailure err -> failwith "failed"
        result = i32
    [<Property>]
    static member ``Parse int64 from array of bytes`` (i64 : int64) =
        let bytes = System.BitConverter.GetBytes i64
        let bParser = BinaryParser.parseInt64
        let result =
            match BinaryParser.run bytes bParser with
            |ParseSuccess byte -> byte
            |ParseFailure err -> failwith "failed"
        result = i64
    [<Property>]
    static member ``Parse float64 from array of bytes`` (flt : float) =
        let bytes = System.BitConverter.GetBytes flt
        let bParser = BinaryParser.parseFloat64
        let result =
            match BinaryParser.run bytes bParser with
            |ParseSuccess byte -> byte
            |ParseFailure err -> failwith "failed"
        match result with
        |x when System.Double.IsNaN(x) -> System.Double.IsNaN(flt)
        |_ -> result = flt
    [<Property>]
    static member ``Parse float32 from array of bytes`` (flt : float32) =
        let bytes = System.BitConverter.GetBytes flt
        let bParser = BinaryParser.parseFloat32
        let result =
            match BinaryParser.run bytes bParser with
            |ParseSuccess byte -> byte
            |ParseFailure err -> failwith "failed"
        match result with
        |x when System.Single.IsNaN(x) -> System.Single.IsNaN(flt)
        |_ -> result = flt
//    [<Property>]
//    static member ``readCharfrom array of one char`` (character : char) =
//        let bytes = System.BitConverter.GetBytes(character)
//        let result = 
//            match BinaryIO.run (MemoryBlockRead (bytes, BinaryIO.readByte)) with
//            |IOSuccess (byte,_) -> character
//            |IOError (_) -> failwith "error"
//        result = character
//    [<Property>]
//    static member ``readDecimal from array of one decimal`` (dec : decimal) =
//        let memoryStream = new System.IO.MemoryStream()
//        let binaryWriter = new System.IO.BinaryWriter(memoryStream);
//        binaryWriter.Write(dec)
//        let bytes = memoryStream.ToArray()
//        let result = 
//            match BinaryIO.run (MemoryBlockRead (bytes, BinaryIO.readDecimal)) with
//            |IOSuccess (dec,_) -> dec
//            |IOError (_) -> failwith "error"
//        result = dec
//    [<Property>]
//    static member ``readFloat from array of one float`` (flt : float) =
//        let bytes = System.BitConverter.GetBytes flt
//        let result = 
//            match BinaryIO.run (MemoryBlockRead (bytes, BinaryIO.readFloat)) with
//            |IOSuccess (flt,_) -> flt
//            |IOError (_) -> failwith "error"
//        match flt with
//        |_ when System.Double.IsNaN flt -> System.Double.IsNaN result
//        |_ -> result = flt
//    [<Property>]
//    static member ``readFloat32 from array of one float32`` (flt : float32) =
//        let bytes = System.BitConverter.GetBytes flt
//        let result = 
//            match BinaryIO.run (MemoryBlockRead (bytes, BinaryIO.readFloat32)) with
//            |IOSuccess (flt,_) -> flt
//            |IOError (_) -> failwith "error"
//        match flt with
//        |_ when System.Single.IsNaN flt -> System.Single.IsNaN result
//        |_ -> result = flt
//    [<Property>]
//    static member ``readInt16 from array of one int16`` (int : int16) =
//        let bytes = System.BitConverter.GetBytes int
//        let result = 
//            match BinaryIO.run (MemoryBlockRead (bytes, BinaryIO.readInt16)) with
//            |IOSuccess (int,_) -> int
//            |IOError (_) -> failwith "error"
//        result = int
//    [<Property>]
//    static member ``readInt32 from array of one int32`` (int : int32) =
//        let bytes = System.BitConverter.GetBytes int
//        let result = 
//            match BinaryIO.run (MemoryBlockRead (bytes, BinaryIO.readInt32)) with
//            |IOSuccess (int,_) -> int
//            |IOError (_) -> failwith "error"
//        result = int
//    [<Property>]
//    static member ``readInt64 from array of one int64`` (int : int64) =
//        let bytes = System.BitConverter.GetBytes int
//        let result = 
//            match BinaryIO.run (MemoryBlockRead (bytes, BinaryIO.readInt64)) with
//            |IOSuccess (int,_) -> int
//            |IOError (_) -> failwith "error"
//        result = int
//    [<Property>]
//    static member ``readString from array of one string`` (str : NonNull<string>) =
//        let str = str.Get
//        use memoryStream = new System.IO.MemoryStream()
//        use binaryWriter = new System.IO.BinaryWriter(memoryStream);
//        binaryWriter.Write(str)
//        let bytes = memoryStream.ToArray()
//        let result = 
//            match BinaryIO.run (MemoryBlockRead (bytes, BinaryIO.readString)) with
//            |IOSuccess (dec,_) -> str
//            |IOError (_) -> failwith "error"
//        result = str
//
//module BinaryIOWriteTester =
//    let getTestResult filename func =
//        let mmapf = System.IO.MemoryMappedFiles.MemoryMappedFile.OpenExisting(filename)
//        use reader = new System.IO.BinaryReader(mmapf.CreateViewStream())
//        func reader
//
//type ``Binary IO Simple Write Tests`` =
//    [<Property>]
//    static member ``writeByte from one byte`` (byte : byte) =
//        let mmapFID = System.IO.Path.GetRandomFileName()
//        match BinaryIO.run (MemoryMappedFileWrite (mmapFID, 1L, BinaryIO.writeByte byte)) with
//        |IOSuccess (_) -> 
//            BinaryIOWriteTester.getTestResult mmapFID (fun br -> br.ReadByte()) = byte
//        |IOError (_) -> failwith "error"
//    [<Property>]
//    static member ``writeChar from one char`` (character : char) =
//        let mmapFID = System.IO.Path.GetRandomFileName()
//        match BinaryIO.run (MemoryMappedFileWrite (mmapFID, 5L, BinaryIO.writeChar character)) with
//        |IOSuccess (_) -> 
//            BinaryIOWriteTester.getTestResult mmapFID (fun br -> br.ReadChar()) = character
//        |IOError (_) -> failwith "error"
//    [<Property>]
//    static member ``writeDecimal from one decimal`` (dec : decimal) =
//        let mmapFID = System.IO.Path.GetRandomFileName()
//        match BinaryIO.run (MemoryMappedFileWrite (mmapFID, int64 sizeof<decimal>, BinaryIO.writeDecimal dec)) with
//        |IOSuccess (_) -> 
//            BinaryIOWriteTester.getTestResult mmapFID (fun br -> br.ReadDecimal()) = dec
//        |IOError (_) -> failwith "error"
//    [<Property>]
//    static member ``writeFloat from one float`` (flt : float) =
//        let mmapFID = System.IO.Path.GetRandomFileName()
//        match BinaryIO.run (MemoryMappedFileWrite (mmapFID, int64 sizeof<float>, BinaryIO.writeFloat flt)) with
//        |IOSuccess (_) -> 
//            let rFlt = BinaryIOWriteTester.getTestResult mmapFID (fun br -> br.ReadDouble())
//            match rFlt with
//            |_ when System.Double.IsNaN rFlt -> System.Double.IsNaN flt
//            |_ -> rFlt = flt
//        |IOError (_) -> failwith "error"
//    [<Property>]
//    static member ``writeFloat32 from one float32`` (flt32 : float32) =
//        let mmapFID = System.IO.Path.GetRandomFileName()
//        match BinaryIO.run (MemoryMappedFileWrite (mmapFID, int64 sizeof<float32>, BinaryIO.writeFloat32 flt32)) with
//        |IOSuccess (_) -> 
//            let rFlt = BinaryIOWriteTester.getTestResult mmapFID (fun br -> br.ReadSingle())
//            match rFlt with
//            |_ when System.Single.IsNaN rFlt -> System.Single.IsNaN flt32
//            |_ -> rFlt = flt32
//        |IOError (_) -> failwith "error"
//    [<Property>]
//    static member ``writeInt16 from one int16`` (i16 : int16) =
//        let mmapFID = System.IO.Path.GetRandomFileName()
//        match BinaryIO.run (MemoryMappedFileWrite (mmapFID, int64 sizeof<int16>, BinaryIO.writeInt16 i16)) with
//        |IOSuccess (_) -> 
//            BinaryIOWriteTester.getTestResult mmapFID (fun br -> br.ReadInt16()) = i16
//        |IOError (_) -> failwith "error"
//    [<Property>]
//    static member ``writeInt32 from one int32`` (i32 : int32) =
//        let mmapFID = System.IO.Path.GetRandomFileName()
//        match BinaryIO.run (MemoryMappedFileWrite (mmapFID, int64 sizeof<int32>, BinaryIO.writeInt32 i32)) with
//        |IOSuccess (_) -> 
//            BinaryIOWriteTester.getTestResult mmapFID (fun br -> br.ReadInt32()) = i32
//        |IOError (_) -> failwith "error"
//    [<Property>]
//    static member ``writeInt64 from one int64`` (i64 : int64) =
//        let mmapFID = System.IO.Path.GetRandomFileName()
//        match BinaryIO.run (MemoryMappedFileWrite (mmapFID, int64 sizeof<int64>, BinaryIO.writeInt64 i64)) with
//        |IOSuccess (_) -> 
//            BinaryIOWriteTester.getTestResult mmapFID (fun br -> br.ReadInt64()) = i64
//        |IOError (_) -> failwith "error"
    
        


