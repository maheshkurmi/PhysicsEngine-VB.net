Option Strict Off
Public Class Vector
    Public x As Double
    Public y As Double
    Public Sub New(px As Double, py As Double)
        x = px
        y = py
    End Sub
    Public Sub setTo(px As Double, py As Double)
        x = px
        y = py
    End Sub
    Public Sub copy(v As Vector)
        x = v.x
        y = v.y
    End Sub
    Public Function dot(v As Vector) As Double
        Return x * v.x + y * v.y
    End Function
    Public Function cross(v As Vector) As Double
        Return x * v.y - y * v.x
    End Function
    Public Function plus(v As Vector) As Vector
        Return New Vector(x + v.x, y + v.y)
    End Function
    Public Function plusEquals(v As Vector) As Vector
        x += v.x
        y += v.y
        Return Me
    End Function
    Public Function minus(v As Vector) As Vector
        Return New Vector(x - v.x, y - v.y)
    End Function
    Public Function minusEquals(v As Vector) As Vector
        x -= v.x
        y -= v.y
        Return Me
    End Function
    Public Function mult(s As Double) As Vector
        Return New Vector(x * s, y * s)
    End Function
    Public Function multEquals(s As Double) As Vector
        x *= s
        y *= s
        Return Me
    End Function
    Public Function times(v As Vector) As Vector
        Return New Vector(x * v.x, y * v.y)
    End Function
    Public Function divEquals(s As Double) As Vector
        If s = 0 Then
            s = 0.0001
        End If
        x /= s
        y /= s
        Return Me
    End Function
    Public Function magnitude() As Double
        Return Math.Sqrt(x * x + y * y)
    End Function
    Public Function distance(v As Vector) As Double
        Dim delta As Vector = Me.minus(v)
        Return delta.magnitude()
    End Function
    Public Function normalize() As Vector
        Dim m As Double = magnitude()
        If m = 0 Then
            m = 0.0001
        End If
        Return mult(1 / m)
    End Function
    Public Overrides Function toString() As String
        Return (x + " : " + y)
    End Function
End Class


