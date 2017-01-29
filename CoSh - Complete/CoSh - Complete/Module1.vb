Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.IO
Imports System.Security.Cryptography.X509Certificates
Imports System.Threading

Module Module1
    'Globals
    Dim cki As ConsoleKeyInfo
    Const noItems As Integer = 10
    ReadOnly itemName As New Dictionary(Of Integer, String) 'Int for item code, string for name
    ReadOnly itemPrice As New Dictionary(Of Integer, Decimal) 'Int for item code, dec for price
    Dim receiptName As New List(Of String) 'string for name
    Dim receiptPrice As New List(Of Decimal) 'dec for price
    Dim menuSelection() As Integer = {1, (noItems - 1)}
    Dim stats As New Dictionary(Of String, Decimal) 'Stores all the values calculated from database
    Dim endDraw1, endDraw2, endDraw3 As Boolean
    Dim totals(2) As Decimal '0=sub-total, 1=vat, 2=total

    Sub Main()
        'Stage
        Console.CursorVisible = False
        Colours("default")
        Console.Clear()

        'Default values/initialisers
        Database("true") 'Read
        endDraw1 = False
        endDraw2 = False
        totals(1) = 0.2 'current VAT
        totals(0) = 0
        totals(2) = 0
        'Add more if necessary

        ASCII("splash")
        Console.Beep(3200, 200)

        'Console size
        'Width = 80
        'Height = 25



        '<-------------Menu page 1---------------------------------------------------------------------------->
        'Menu title
        ASCII("menu")
        'Duds:
        Console.SetCursorPosition(14, 11) 'Halfway down the left half
        Colours("highlight")
        Console.WriteLine("Create Order")
        Colours("default")
        Console.SetCursorPosition(14, 12)
        Console.WriteLine("Close Program")

        'Border Line
        ASCII("border")
        ASCII("helpBorder")

        'Help
        ASCII("menu1Help")


        'Show stats + title
        ASCII("stats")


        'User input
        Do While endDraw1 = False
            DrawMenu1()
        Loop

        'We can assume if the user is past this point then they want to make an order
        '<-------------End of page 1-------------------------------------------------------------------------->



        '<-------------Menu page 2---------------------------------------------------------------------------->
        'Stage
        Console.Clear()
        ASCII("border")
        ASCII("items")
        ASCII("receipt")
        ASCII("menu2Help")
        menuSelection(0) = 0
        menuSelection(1) = 0


        'Duds
        '<------Menu------>
        For i = 1 To noItems
            Static j = 7
            If j = 7 Then
                Colours("highlight")
            Else
                Colours("default")
            End If
            Console.SetCursorPosition(10, j)
            Console.WriteLine(itemName(i - 1)) 'Items

            Console.SetCursorPosition(4, j)
            Console.Write(FormatCurrency(itemPrice(i - 1), , , TriState.True, TriState.True)) 'Prices
            j += 1
        Next

        'Totals
        ASCII("totalDuds")

        '<------Receipt--->
        For Each item In receiptName
            Static k = 7
            Console.SetCursorPosition(45, k)
            Console.Write(receiptName(k - 7))
            Console.SetCursorPosition(70, k)
            Console.Write(FormatCurrency(receiptPrice(k - 7), , , TriState.True, TriState.True))
            If k <> receiptName.Count() Then
                k += 1
            End If
        Next

        'Help



        'MOVE THIS
        ASCII("helpBorder")


        'User input
        Do While endDraw2 = False
            DrawMenu2()
        Loop

        Colours("default")
        Console.Clear()

        '<--------Change calc----------->
        ASCII("border")
        ASCII("helpBorder")
        ASCII("change")
        'Help
        Console.SetCursorPosition(2, 22)
        Console.Write("Use the ")
        Colours("highlight")
        Console.Write("Arrow")
        Colours("default")
        Console.Write(" keys to select.")
        Console.SetCursorPosition(2, 23)
        Colours("highlight")
        Console.Write("Enter")
        Colours("default")
        Console.Write(" key to confirm selection.")

        Console.SetCursorPosition(5, 10)
        Console.Write("Do you want to calculate change?")

        'Duds
        Console.SetCursorPosition(10, 12)
        Colours("highlight")
        Console.Write("Yes")
        Colours("default")
        Console.SetCursorPosition(10, 13)
        Console.Write("No")
        'Receipt
        For i = 0 To (receiptName.Count() - 1)
            Console.SetCursorPosition(45, (i + 7))
            Console.Write(receiptName(i))
            Console.SetCursorPosition(70, (i + 7))
            Console.Write(FormatCurrency(receiptPrice(i), , , TriState.True, TriState.True))
        Next
        ASCII("receipt")
        'Totals
        ASCII("totalDuds")
        Console.SetCursorPosition(70, 21)
        Console.Write(FormatCurrency(totals(0), , , TriState.True, TriState.True)) 'Sub-total
        Console.SetCursorPosition(70, 23)
        Console.Write(FormatCurrency(totals(2), , , TriState.True, TriState.True)) 'Total


        Do While endDraw3 = False
            cki = Console.ReadKey()
            Static yesNo As Boolean = True
            If cki.Key = ConsoleKey.UpArrow Then
                Console.SetCursorPosition(10, 12)
                Colours("highlight")
                Console.Write("Yes")
                Colours("default")
                Console.SetCursorPosition(10, 13)
                Console.Write("No")

                yesNo = True

            ElseIf cki.Key = ConsoleKey.DownArrow Then
                Console.SetCursorPosition(10, 12)
                Colours("default")
                Console.Write("Yes")
                Colours("highlight")
                Console.SetCursorPosition(10, 13)
                Console.Write("No")
                Colours("default")

                yesNo = False

            ElseIf cki.Key = ConsoleKey.Enter Then

                If yesNo = True Then
                    Colours("default")
                    Console.Clear()
                    ASCII("border")
                    'Help
                    ASCII("helpBorder")
                    'Receipt
                    For i = 0 To (receiptName.Count() - 1)
                        Console.SetCursorPosition(45, (i + 7))
                        Console.Write(receiptName(i))
                        Console.SetCursorPosition(70, (i + 7))
                        Console.Write(FormatCurrency(receiptPrice(i), , , TriState.True, TriState.True))
                    Next
                    ASCII("receipt")
                    ASCII("change")

                    'Help
                    Console.SetCursorPosition(2, 22)
                    Console.Write("Type in the value of money given.")
                    Console.SetCursorPosition(2, 23)
                    Colours("RK")
                    Console.Write("NUMBERS ONLY! (Excluding '.')")
                    Colours("default")

                    'Totals
                    ASCII("totalDuds")
                    Console.SetCursorPosition(70, 21)
                    Console.Write(FormatCurrency(totals(0), , , TriState.True, TriState.True)) 'Sub-total
                    Console.SetCursorPosition(70, 23)
                    Console.Write(FormatCurrency(totals(2), , , TriState.True, TriState.True)) 'Total

                    Console.SetCursorPosition(10, 10)
                    Console.Write("Enter money given:")
                    Console.SetCursorPosition(10, 11)
                    Console.Write("£")

                    Console.CursorVisible = True
                    Dim mIn As Decimal = Console.ReadLine()
                    Console.CursorVisible = False

                    Console.SetCursorPosition(10, 12)
                    Console.Write("Change to be given:")
                    Console.SetCursorPosition(10, 13)
                    Console.Write(FormatCurrency(ChangeCalc(mIn, totals(2)), , , TriState.True, TriState.True))



                End If

                endDraw3 = True


                Console.SetCursorPosition(1, 22)
                Console.Write("                                       ") 'Clear artifacts
                Console.SetCursorPosition(1, 23)
                Console.Write("                                       ")
                Console.SetCursorPosition(2, 22)
                Console.Write("Press the ")
                Colours("highlight")
                Console.Write("Enter")
                Colours("default")
                Console.Write(" key")
                Console.SetCursorPosition(2, 23)
                Console.Write("to complete order.")
                Console.ReadLine()
                Database(False) 'Write to file
                Console.Beep() 'To signify the end of the program
            End If
        Loop





        'Console.ReadLine() 'Temp
    End Sub

    Sub Colours(colour As String)
        'Colours available:             ~Background then foreground
        'Dark blue & Cyan = "default"
        'Cyan & Dark blue = "highlight"
        'White & Blue = WB
        'Red & Black = RB
        'Dark red & dark yellow = DN

        Select Case colour
            Case "default"
                Console.BackgroundColor = ConsoleColor.DarkBlue
                Console.ForegroundColor = ConsoleColor.Cyan

            Case "highlight"
                Console.BackgroundColor = ConsoleColor.Cyan
                Console.ForegroundColor = ConsoleColor.DarkBlue

            Case "WB"
                Console.BackgroundColor = ConsoleColor.White
                Console.ForegroundColor = ConsoleColor.Blue

            Case "RK"
                Console.BackgroundColor = ConsoleColor.Red
                Console.ForegroundColor = ConsoleColor.Black

            Case "DN"
                Console.BackgroundColor = ConsoleColor.DarkRed
                Console.ForegroundColor = ConsoleColor.DarkYellow

            Case "RW"
                Console.BackgroundColor = ConsoleColor.White
                Console.ForegroundColor = ConsoleColor.DarkRed
        End Select


    End Sub

    Sub ASCII(type As String)
        'This sub draws ASCII art in the console

        Select Case type
            Case "splash"
                Colours("DN") 'Dark red with dark yellow
                Console.Clear()
                Console.WriteLine("                              (")
                Console.WriteLine("                                )     (")
                Console.WriteLine("                         ___...(-------)-....___")
                Console.WriteLine("                     .-''       )    (          ''-.")
                Console.WriteLine("               .-'``'|-._             )         _.-|")
                Console.WriteLine("              /  .--.|   `''---...........---''`   |")
                Console.WriteLine("             /  /    |                             |")
                Console.WriteLine("             |  |    |                             |")
                Console.WriteLine("              \  \   |                             |")
                Console.WriteLine("               `\ `\ |                             |")
                Console.WriteLine("                 `\ `|                             |")
                Console.WriteLine("                 _/ /\                             /")
                Console.WriteLine("                (__/  \                           /")
                Console.WriteLine("             _..---''` \                         /`''---.._")
                Console.WriteLine("          .-'           \                       /          '-.")
                Console.WriteLine("         :               `-.__             __.-'              :")
                Console.WriteLine("         :                  ) ''---...---'' (                 :")
                Console.WriteLine("          '._               `'--...___...--'`              _.'")
                Console.WriteLine("            \''--..__                              __..--''/")
                Console.WriteLine("             '._     '''----.....______.....----'''     _.'")
                Console.WriteLine("                `''--..,,_____            _____,,..--''`")
                Console.WriteLine("                              `'''----'''`")
                Console.WriteLine("                              TweakCoffee")
                Colours("default")

                Thread.Sleep(2000)
                Console.Clear()

            Case "menu"
                Console.SetCursorPosition(8, 1)
                Console.WriteLine("   __  ___             ")
                Console.SetCursorPosition(8, 2)
                Console.WriteLine("  /  |/  /__ ___  __ __")
                Console.SetCursorPosition(8, 3)
                Console.WriteLine(" / /|_/ / -_) _ \/ // /")
                Console.SetCursorPosition(8, 4)
                Console.WriteLine("/_/  /_/\__/_//_/\_,_/ ")
                Console.SetCursorPosition(8, 5)
                Console.WriteLine("___________________________")

            Case "stats"
                Console.SetCursorPosition(45, 1)
                Console.WriteLine("   ______       __    ")
                Console.SetCursorPosition(45, 2)
                Console.WriteLine("  / __/ /____ _/ /____")
                Console.SetCursorPosition(45, 3)
                Console.WriteLine(" _\ \/ __/ _ `/ __(_-<")
                Console.SetCursorPosition(45, 4)
                Console.WriteLine("/___/\__/\_,_/\__/___/")
                Console.SetCursorPosition(45, 5)
                Console.WriteLine("________________________")
                'Today
                Console.SetCursorPosition(45, 7)
                Console.Write("Today's total income:")
                Console.SetCursorPosition(71, 7)
                Console.Write(FormatCurrency(stats("dayTotal"), , , TriState.True, TriState.True))
                Console.SetCursorPosition(45, 8)
                Console.Write("Today's customers:")
                Console.SetCursorPosition(71, 8)
                Console.Write(stats("dayCustomers"))
                Console.SetCursorPosition(45, 9)
                Console.Write("Avg spending price today:")
                Console.SetCursorPosition(71, 9)
                Console.Write(FormatCurrency(stats("dayAverSpend"), , , TriState.True, TriState.True))
                Console.SetCursorPosition(45, 10)
                Console.Write("Items sold today:")
                Console.SetCursorPosition(71, 10)
                Console.Write(stats("dayItemsSold"))
                'All time
                Console.SetCursorPosition(45, 15)
                Console.Write("Total income:")
                Console.SetCursorPosition(71, 15)
                Console.Write(FormatCurrency(stats("allTotal"), , , TriState.True, TriState.True))
                Console.SetCursorPosition(45, 16)
                Console.Write("Total customers:")
                Console.SetCursorPosition(71, 16)
                Console.Write(stats("allCustomers"))
                Console.SetCursorPosition(45, 17)
                Console.Write("Avg spending price:")
                Console.SetCursorPosition(71, 17)
                Console.Write(FormatCurrency(stats("allAverSpend"), , , TriState.True, TriState.True))
                Console.SetCursorPosition(45, 18)
                Console.Write("Items sold:")
                Console.SetCursorPosition(71, 18)
                Console.Write(stats("allItemsSold"))



            Case "border"
                For i = 0 To 24
                    Console.SetCursorPosition(40, i)
                    Console.Write("|")
                Next

            Case "items"
                Console.SetCursorPosition(8, 1)
                Console.WriteLine("   ______              ")
                Console.SetCursorPosition(8, 2)
                Console.WriteLine("  /  _/ /____ __ _  ___")
                Console.SetCursorPosition(8, 3)
                Console.WriteLine(" _/ // __/ -_)  ' \(_-<")
                Console.SetCursorPosition(8, 4)
                Console.WriteLine("/___/\__/\__/_/_/_/___/")
                Console.SetCursorPosition(8, 5)
                Console.WriteLine("_________________________")

            Case "receipt"
                Console.SetCursorPosition(45, 1)
                Console.WriteLine("   ___              _      __ ")
                Console.SetCursorPosition(45, 2)
                Console.WriteLine("  / _ \___ _______ (_)__  / /_")
                Console.SetCursorPosition(45, 3)
                Console.WriteLine(" / , _/ -_) __/ -_) / _ \/ __/")
                Console.SetCursorPosition(45, 4)
                Console.WriteLine("/_/|_|\__/\__/\__/_/ .__/\__/ ")
                Console.SetCursorPosition(45, 5)
                Console.WriteLine("__________________/_/___________")

            Case "helpBorder"
                For i = 0 To 79
                    Console.SetCursorPosition(i, 20)
                    Console.Write("_")
                Next
                Console.SetCursorPosition(0, 21)
                Colours("RW")                              '          '
                Console.Write(" Help:                        " & DateTime.Now.ToString("dd/MM/yyyy"))
                Colours("default")


            Case "clearReceipt"
                'Clears whatever was in the space before
                Colours("default")
                Console.SetCursorPosition(45, (receiptName.Count() + 7)) 'Location of last deleted item
                Console.Write("                                   ")

            Case "menu1Help"
                Console.SetCursorPosition(1, 22)
                Console.Write("The ")
                Colours("highlight")
                Console.Write("Arrow")
                Colours("default")
                Console.Write(" Keys are used for navigation,")
                Console.SetCursorPosition(1, 23)
                Colours("highlight")
                Console.Write("Enter")
                Colours("default")
                Console.Write(" key to select.")

            Case "menu2Help"
                Console.SetCursorPosition(1, 22)
                Colours("highlight")
                Console.Write("Arrow")
                Colours("default")
                Console.Write(" keys are used for navigation,")
                Console.SetCursorPosition(1, 23)
                Colours("highlight")
                Console.Write("Enter")
                Colours("default")
                Console.Write(" key to Add/Remove items.")
                Console.SetCursorPosition(1, 24)
                Colours("highlight")
                Console.Write("ESC")
                Colours("default")
                Console.Write(" key to exit, ")
                Colours("highlight")
                Console.Write("SPACE")
                Colours("default")
                Console.Write(" key to confirm.")

            Case "totalDuds"
                Console.SetCursorPosition(45, 21)
                Console.Write("Sub-Total: ")
                Console.SetCursorPosition(45, 22)
                Console.Write("VAT: ")
                Console.SetCursorPosition(45, 23)
                Console.Write("Total: ")
                Console.SetCursorPosition(70, 21)
                Console.Write(FormatCurrency(totals(0), , , TriState.True, TriState.True)) 'Sub-total
                Console.SetCursorPosition(70, 22)
                Console.Write((totals(1) * 100).ToString() & "%") 'VAT
                Console.SetCursorPosition(70, 23)
                Console.Write(FormatCurrency(totals(2), , , TriState.True, TriState.True)) 'Total

            Case "change"
                Console.SetCursorPosition(5, 1)
                Console.WriteLine("  _______                     ")
                Console.SetCursorPosition(5, 2)
                Console.WriteLine(" / ___/ /  ___ ____  ___ ____ ")
                Console.SetCursorPosition(5, 3)
                Console.WriteLine("/ /__/ _ \/ _ `/ _ \/ _ `/ -_)")
                Console.SetCursorPosition(5, 4)
                Console.WriteLine("\___/_//_/\_,_/_//_/\_, /\__/ ")
                Console.SetCursorPosition(5, 5) '      '
                Console.WriteLine("____________________/__/_______")

        End Select


    End Sub

    Sub DrawMenu1()
        Static keySelected As Single 'remembers the current selection through each loop

        cki = Console.ReadKey() 'watch input keys

        If cki.Key = ConsoleKey.UpArrow Then
            Console.SetCursorPosition(14, 11)
            Colours("highlight")
            Console.WriteLine("Create Order")
            Colours("default")
            Console.SetCursorPosition(14, 12)
            Console.WriteLine("Close Program")

            keySelected = 0

        ElseIf cki.Key = ConsoleKey.DownArrow Then
            Console.SetCursorPosition(14, 11)
            Colours("default")
            Console.WriteLine("Create Order")
            Colours("highlight")
            Console.SetCursorPosition(14, 12)
            Console.WriteLine("Close Program")
            Colours("default")

            keySelected = 1

        ElseIf cki.Key = ConsoleKey.Enter Then
            If keySelected = 1 Then
                Environment.Exit(0)
            Else
                Console.Clear()
                endDraw1 = True
                Exit Sub
            End If
        End If
    End Sub

    Sub DrawMenu2()
        'menuSelection describes which item is selected, there are two fields
        '(x,y) - x is the position horizontally, either 0 or 1 as there are two lists
        '      - y is the position vertically on either lists
        'menuSelection(0) = 0 < ----horizontal
        'menuSelection(1) = 0 < ----vertical

        'menuSelection(1) goes up to noItems-1, default is 9

        cki = Console.ReadKey() 'Watch input keys

        Select Case cki.Key
            Case ConsoleKey.UpArrow
                If menuSelection(1) >= 1 Then 'Upper limit
                    menuSelection(1) -= 1
                End If

            Case ConsoleKey.DownArrow
                If menuSelection(0) = 0 Then 'Lower limit
                    If menuSelection(1) < (noItems - 1) Then
                        menuSelection(1) += 1
                    End If
                Else
                    If menuSelection(1) < (receiptName.Count - 1) Then
                        menuSelection(1) += 1
                    End If

                End If

            Case ConsoleKey.LeftArrow
                If menuSelection(1) > 9 Then 'Needed because the receipt list can be bigger
                    menuSelection(1) = 9
                End If

                menuSelection(0) = 0

            Case ConsoleKey.RightArrow
                If menuSelection(1) > (receiptName.Count() - 1) Then 'Needed if items list is bigger
                    menuSelection(1) = (receiptName.Count() - 1)
                End If

                menuSelection(0) = 1

            Case ConsoleKey.Escape       'Exit
                Environment.Exit(0)

            Case ConsoleKey.Enter
                If Not menuSelection(0) = 0 Then 'When on the receipt list
                    Total(False, menuSelection(1)) 'Changes and prints totals
                    '^^^^^^^HAS to be above vvvvv
                    receiptName.RemoveAt(menuSelection(1)) 'Remove items
                    receiptPrice.RemoveAt(menuSelection(1))
                    ASCII("clearReceipt")
                    If menuSelection(1) >= receiptName.Count() Then
                        If receiptName.Count() = 0 Then
                            menuSelection(1) = 0
                            menuSelection(0) = 0
                        Else
                            menuSelection(1) -= 1
                        End If

                    End If


                Else 'When on the item list
                    If receiptName.Count() < 12 Then '<----------------------Limits list to 12
                        receiptName.Add(itemName(menuSelection(1))) 'Adds item to receipt
                        receiptPrice.Add(itemPrice(menuSelection(1))) 'Adds item's price to receipt

                        Total(True, menuSelection(1)) 'Changes and prints totals

                    End If
                End If

            Case ConsoleKey.Spacebar
                endDraw2 = True


        End Select

        Highlight(menuSelection(0), menuSelection(1)) 'Highlights selected item



        'endDraw2 = True 'placeholder
    End Sub

    Sub Highlight(xSelection As Integer, ySelection As Integer)
        '<------------->
        'Warning: There are many different variable names in this sub, each single letter variable is independant and
        'used only within the containing loops/conditional statements
        '<------------->


        If xSelection = 0 Then
            '<----------------------item list------------------------------------------>
            For i = 1 To noItems 'i represents each item on the item list
                Static j = 7 'j represents the position to write each item
                If (j - 7) = ySelection Then
                    Colours("highlight")
                Else
                    Colours("default")
                End If
                Console.SetCursorPosition(10, j)
                Console.Write(itemName(i - 1)) 'Items

                Console.SetCursorPosition(4, j)
                Console.Write(FormatCurrency(itemPrice(i - 1), , , TriState.True, TriState.True)) 'Prices

                If j = (noItems + 6) Then
                    j = 7
                Else
                    j += 1
                End If
            Next
            '<----------------------end------------------------------------------------>

            '<--------------------receipt list----------------------------------------->
            For i = 1 To receiptName.Count()
                Static k = 7

                Colours("default") 'Just in case

                Console.SetCursorPosition(45, k)
                Console.Write("                                   ") 'Clears artifacts
                Console.SetCursorPosition(45, k)
                Console.Write(receiptName(i - 1)) 'Items

                Console.SetCursorPosition(70, k)
                Console.Write(FormatCurrency(receiptPrice(i - 1), , , TriState.True, TriState.True)) 'Prices

                If k = (receiptName.Count() + 6) Then
                    k = 7
                Else
                    k += 1
                End If
            Next
            '<----------------------end------------------------------------------------>

        Else
            '<-----This is needed to 'move' the cursor, it unhighlights everything on the left------->
            '<----------------------item list-------------------------------------------------------->
            For i = 1 To noItems 'i represents each item on the item list
                Static l = 7 'l represents the position to write each item
                Colours("default")

                Console.SetCursorPosition(10, l)
                Console.Write(itemName(i - 1)) 'Items

                Console.SetCursorPosition(4, l)
                Console.Write(FormatCurrency(itemPrice(i - 1), , , TriState.True, TriState.True)) 'Prices
                If l = (noItems + 6) Then
                    l = 7
                Else
                    l += 1
                End If
            Next
            '<----------------------end-------------------------------------------------------------->

            '<--------------------receipt list------------------------------------------------------->
            For i = 1 To receiptName.Count() 'i represents each item on the item list
                Static m = 7 'm represents the position to write each item
                If (m - 7) = ySelection Then
                    Colours("highlight")
                Else
                    Colours("default")
                End If
                Console.SetCursorPosition(45, m)
                Console.Write("                                   ") 'Clears artifacts
                Console.SetCursorPosition(45, m)
                Console.Write(receiptName(i - 1)) 'Items

                Console.SetCursorPosition(70, m)
                Console.Write(FormatCurrency(receiptPrice(i - 1), , , TriState.True, TriState.True)) 'Prices

                If m = (receiptName.Count() + 6) Then
                    m = 7
                Else
                    m += 1
                End If
            Next
            '<----------------------end-------------------------------------------------------------->

        End If

    End Sub

    Sub Total(addRemove As Boolean, x As Single)
        If addRemove = True Then 'True being ADD, false being REMOVE
            totals(0) += itemPrice(x)

        ElseIf addRemove = False Then
            totals(0) -= receiptPrice(x)
        End If

        totals(2) = totals(0) * (totals(1) + 1) 'Sub-total * current VAT(0.2) +1 to make 1.20/120% of sub-total

        'Print
        Colours("default")
        Console.SetCursorPosition(70, 21)
        Console.Write("      ") 'Artifact cleaning
        Console.SetCursorPosition(70, 21)
        Console.Write(FormatCurrency(totals(0), , , TriState.True, TriState.True)) 'Sub-total
        Console.SetCursorPosition(70, 23)
        Console.Write("      ") 'Artifact cleaning
        Console.SetCursorPosition(70, 23)
        Console.Write(FormatCurrency(totals(2), , , TriState.True, TriState.True)) 'Total

    End Sub

    Sub Database(inOut As Boolean)

        If inOut = True Then 'Reading data

            If Not My.Settings.DateOfLastOrder = DateTime.Now.ToString("dd/MM/yyyy") Then
                My.Settings.DayTotal = 0
                My.Settings.DayCustomers = 0
                My.Settings.DayAverageSpend = 0
                My.Settings.DayItemsSold = 0

                My.Settings.DateOfLastOrder = DateTime.Now.ToString("dd/MM/yyyy")

                My.Settings.Save()
            End If

            'Today
            stats.Add("dayTotal", Math.Round(My.Settings.DayTotal, 2))   '<------- Make these load from database
            stats.Add("dayCustomers", Math.Round(My.Settings.DayCustomers, 2))
            stats.Add("dayAverSpend", Math.Round(My.Settings.DayAverageSpend, 2))
            stats.Add("dayItemsSold", Math.Round(My.Settings.DayItemsSold, 2))

            'All time
            stats.Add("allTotal", Math.Round(My.Settings.AllTotal, 2))   '<------- Make these load from database
            stats.Add("allCustomers", Math.Round(My.Settings.AllCustomers, 2))
            stats.Add("allAverSpend", Math.Round(My.Settings.AllAverageSpend, 2))
            stats.Add("allItemsSold", Math.Round(My.Settings.AllItemsSold, 2))

            itemName(0) = "Coffee"
            itemName(1) = "Tea"
            itemName(2) = "Muffin"
            itemName(3) = "Toast"
            itemName(4) = "Cappuccino"
            itemName(5) = "Cookie"
            itemName(6) = "Hot Chocolate"
            itemName(7) = "Ethanol"
            itemName(8) = "Carbonated Drink"
            itemName(9) = "Jaffa Cakes"
            Dim rnd As New Random
            For i = 0 To 9
                Dim x As Integer = rnd.Next(0, 501)
                Dim z = x / 100
                itemPrice(i) = z
            Next

        Else 'Writing to storage
            My.Settings.DayTotal += Math.Round(totals(2), 2)
            My.Settings.DayCustomers += 1
            My.Settings.DayItemsSold += Math.Round(receiptName.Count(), 2)
            My.Settings.DayAverageSpend = Math.Round(My.Settings.DayTotal / My.Settings.DayCustomers, 2)

            My.Settings.AllTotal += Math.Round(totals(2), 2)
            My.Settings.AllCustomers += 1
            My.Settings.AllItemsSold += Math.Round(receiptName.Count(), 2)
            My.Settings.AllAverageSpend = Math.Round(My.Settings.AllTotal / My.Settings.AllCustomers, 2)

            My.Settings.Save()

        End If



    End Sub

    Function ChangeCalc(moneyIn As Decimal, cost As Decimal)
        Dim moneyOut As Decimal

        moneyOut = moneyIn - cost

        Return moneyOut
    End Function

End Module
