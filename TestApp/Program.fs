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
open System.Net
open System.Text

[<EntryPoint>]
let main argv = 

    let fName = File.assumeValidFilename "test4.txt"

    let test = io {
        let! lines = File.readLines fName
        return! IO.mapM_ (Console.printfn "%s") (Seq.toList lines)
        }

    let consoleTest = 
        io{
        let! inputStrs = IO.takeWhileM (fun str -> str <> "" |> IO.return') (Console.readLine)
        do! IO.mapM_ (Console.printfn "%s") inputStrs
        }

    let results = IO.run consoleTest

    let httpResponse handle (content : string) =
        let length = System.Text.Encoding.UTF8.GetByteCount(content)
        io {
            do! IO.hPutStrLn handle ("HTTP/1.1 200 OK")
            do! IO.hPutStrLn handle ("Content-Type: text/html")
            do! IO.hPutStrLn handle (sprintf "Content-Length: %d" length)
            do! IO.hPutStrLn handle ("")
            do! IO.hPutStrLn handle (content)
        }

    let testServ = io {
        let! serv = TCP.createServer (System.Net.IPAddress.Any) (7826)
        let! acceptSock = TCP.acceptConnection serv
        let! handle = TCP.socketToHandle acceptSock
        let! request = IO.takeWhileM (fun str -> str <> "" |> IO.return') (IO.hGetLine handle)
        do! httpResponse handle "<html>Test response</html>"
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

     