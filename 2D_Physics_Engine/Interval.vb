Option Strict Off
Public Class Interval
    Public min As Double
    Public max As Double
    Public Sub New(min As Double, max As Double)
        Me.min = min
        Me.max = max
    End Sub
    Public Overloads Function toString() As [String]
        Return (min + " : " + max)
    End Function
End Class


