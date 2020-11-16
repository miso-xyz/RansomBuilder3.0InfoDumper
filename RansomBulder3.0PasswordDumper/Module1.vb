Imports System.IO
Imports System.Reflection
Module Module1
    Sub drawTopLid()
        Console.Write("█")
        For x = 0 To Console.WindowWidth - 3
            Console.Write("▀")
        Next
        Console.WriteLine()
    End Sub
    Sub drawBottomLid()
        Console.Write("█")
        For x = 0 To Console.WindowWidth - 3
            Console.Write("▄")
        Next
        Console.WriteLine()
    End Sub
    Sub drawSplit()
        Console.Write("█")
        For x = 0 To Console.WindowWidth - 3
            Console.Write("─")
        Next
        Console.WriteLine()
    End Sub
    Function alignRight(ByVal data As String, ByVal charFill As Char, ByVal strChar As Char) As String
        Dim line As String = strChar
        For x = 0 To (Console.WindowWidth - 3) - data.Count
            line += charFill
        Next
        line += data
        Return line
    End Function
    Sub Main(ByVal args As String())
        Console.Title = "Ransom Builder 3.0 Info Dumper"
        Dim path
        If args.Count = 0 Then
            Dim err As String = ""
no_args_str:
            Console.Clear()
            Console.Title = "Ransom Builder 3.0 Info Dumper | Waiting for application..."
            drawTopLid()
            If err.Length > 0 Then
                Console.WriteLine("█ " & err)
                drawSplit()
            End If
            Console.WriteLine("█ No arguments found! Please enter the path to the targetted application")
            Console.Write("█ : ")
            Dim aa_ = Console.ReadLine.Replace("""", Nothing)
            If IO.File.Exists(aa_) Then
                Try
                    Dim ab_ As dnlib.DotNet.ModuleDef = dnlib.DotNet.ModuleDefMD.Load(aa_)
                    Dim temp = ab_.RuntimeVersion
                Catch ex As Exception
                    err = "Not a valid assembly (not .NET)"
                    GoTo no_args_str
                End Try
            Else
                err = "Invalid path"
                GoTo no_args_str
            End If
            path = aa_
            Console.Clear()
        Else
            path = args(0)
        End If
        Dim filetypecount As Integer
        Dim unk As Integer
        Dim unk_str As New List(Of String)
        Dim patchedApp As dnlib.DotNet.ModuleDef = dnlib.DotNet.ModuleDefMD.Load(path)
        For x = 0 To patchedApp.Types(1).Methods(0).Body.Instructions.Count - 1
            Console.Title = "Ransom Builder 3.0 Info Dumper | " & x + 1 & "/" & patchedApp.Types(1).Methods(0).Body.Instructions.Count
            If CInt(patchedApp.Types(1).Methods(0).Body.Instructions(x).Offset.ToString) >= "994" AndAlso CInt(patchedApp.Types(1).Methods(0).Body.Instructions(x).Offset.ToString) <= "1038" Then
                If patchedApp.Types(1).Methods(0).Body.Instructions(x).OpCode.ToString = "newarr" Then
                    filetypecount = patchedApp.Types(1).Methods(0).Body.Instructions(x - 1).GetLdcI4Value()
                    drawTopLid()
                    Console.WriteLine("█ Number of file types affected: " & filetypecount)
                    drawSplit()
                End If
            End If
            If patchedApp.Types(1).Methods(0).Body.Instructions(x).OpCode.ToString = "ldstr" Then
                Dim f_found As Integer
                If CInt(patchedApp.Types(1).Methods(0).Body.Instructions(x).Offset.ToString) >= "994" AndAlso CInt(patchedApp.Types(1).Methods(0).Body.Instructions(x).Offset.ToString) <= "1038" Then
                    If f_found <> filetypecount Then
                        Console.WriteLine("█ Filetypes affected (" & f_found + 1 & "): ." & patchedApp.Types(1).Methods(0).Body.Instructions(x).ToString().Split("""")(1))
                        f_found += 1
                        If f_found = filetypecount Then
                            drawSplit()
                        End If
                    ElseIf patchedApp.Types(1).Methods(0).Body.Instructions(x).Offset.ToString = "1028" Then
                        Console.WriteLine("█ Encryption Key: " & patchedApp.Types(1).Methods(0).Body.Instructions(x).ToString().Split("""")(1))
                        drawSplit()
                    ElseIf patchedApp.Types(1).Methods(0).Body.Instructions(x).Offset.ToString = "1033" Then
                        Console.WriteLine("█ Encrypted Files filetype: " & patchedApp.Types(1).Methods(0).Body.Instructions(x).ToString().Split("""")(1))
                    Else
                        unk += 1
                        unk_str.Add(patchedApp.Types(1).Methods(0).Body.Instructions(x).ToString().Split("""")(1))
                    End If
                End If
            End If
        Next
        drawSplit()
        Console.WriteLine("█ Resources (" & patchedApp.Resources.Count & ")")
        For x = 0 To patchedApp.Resources.Count - 1
            Console.WriteLine("█ " & x + 1 & ": " & patchedApp.Resources(x).Name.ToString)
        Next
        If unk > 0 Then
            drawSplit()
            For x = 0 To unk
                Console.WriteLine("█ Unknown string: " & unk_str(x))
            Next
        Else
            drawSplit()
            Console.WriteLine("█ App by misonothx | sinister.ly")
            Console.WriteLine(alignRight("█ Version 1.0", CChar("▄"), CChar("█")))
        End If
        'Console.WriteLine()
        'Console.WriteLine()
        'Console.WriteLine()
        'Console.WriteLine()
        'Console.WriteLine()
        'Console.WriteLine()
        'Console.WriteLine()
        'Console.WriteLine("█▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀█")
        'Console.WriteLine("█────────────────App by misonothx────────────────█")
        'Console.WriteLine("█▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄█")
        Console.ReadKey()
    End Sub

End Module
