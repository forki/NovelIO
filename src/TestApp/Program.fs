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

open NovelFS.NovelIO
open NovelFS.NovelIO.BinaryPickler
open System.Net
open System.Text
open NovelFS.NovelIO.IO



[<EntryPoint>]
let main argv = 

    let fName = File.assumeValidFilename "test4.txt"

    let test = 
        io {
            let! lines = File.readLines fName
            for lineIO in lines do
                let! line = lineIO
                do! (putStrLn <| sprintf "%s%d" line 5)
            let! lSeq = IO.sequence lines
            let! test = IO.mapM (IO.return' << sprintf "%s") lSeq
            return! putStrLn <| sprintf "%A" test
        }

    let consoleTest = 
        io{
            let! inputStrs = IO.Loops.unfoldWhileM (fun str -> str <> "") (Console.readLine)
            do! IO.iterM (putStrLn << sprintf "%s") inputStrs
        }

    let results = IO.run test

    let httpResponse handle (content : string) =
        let length = System.Text.Encoding.UTF8.GetByteCount(content)
        io {
            do! IO.hPutStrLn handle ("HTTP/1.1 200 OK")
            do! IO.hPutStrLn handle ("Content-Type: text/html")
            do! IO.hPutStrLn handle (sprintf "Content-Length: %d" length)
            do! IO.hPutStrLn handle ("")
            do! IO.hPutStrLn handle (content)
        }

    let handleSocket acceptSock =
        io {
            let! handle = TCP.socketToHandle acceptSock
            let! request = IO.Loops.unfoldWhileM (fun str -> str <> "") (IO.hGetLine handle)
            do! httpResponse handle "<html>Test response</html>"
            do! TCP.closeConnection acceptSock
        }

    let testServ = 
        io {
            let! serv = TCP.createServer (System.Net.IPAddress.Any) (7826)
            while true do
                let! sock = TCP.acceptConnection serv
                do! IO.forkIO <| handleSocket sock
                //do! handleSocket sock
            //do! IO.Loops.iterateWhile (fun _ -> true) (IO.forkIO << handleSocket <| TCP.acceptConnection serv)
            //let! acceptSock = TCP.acceptConnection serv
            //let! handle = TCP.socketToHandle acceptSock
            //let! request = IO.Loops.unfoldWhileM (fun str -> str <> "") (IO.hGetLine handle)
            //do! httpResponse handle "<html>Test response</html>"
        }
        

    let test = IO.run testServ

    //let lines = File.assumeValidFilename "test3.txt" |> IO.readLines |> IO.run
    //match lines with
    //|IOSuccess a -> a |> Seq.iter (printfn "%s")
    //|IOError err -> printfn "%A" err


    //let testLines = System.IO.File.ReadLines "test4.txt"

    //testLines |> Seq.iter (printfn "%s")


    //testLines |> Seq.iter (printfn "%s")

    0

     