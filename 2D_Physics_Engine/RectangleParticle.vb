Option Strict Off
Public Class RectangleParticle
    Inherits AbstractParticle
    Public _cornerPositions As New ArrayList()
    Private _cornerParticles As New ArrayList()
    Private _extents As New ArrayList()
    Private _axes As New ArrayList()
    Private _rotation As Double
    Public Sub New(x As Double, y As Double, width As Double, height As Double, rotation As Double, fixed As Boolean, _
     mass As Double, elasticity As Double, friction As Double)
        MyBase.new(x, y, fixed, mass, elasticity, friction)
        _extents.Add(width / 2)
        _extents.Add(height / 2)
        _axes.Add(New Vector(0, 0))
        _axes.Add(New Vector(0, 0))
        setRotation(rotation)
        _cornerPositions = getCornerPositions()
        _cornerParticles = getCornerParticles()
    End Sub
    Public Function getRotation() As Double
        Return _rotation
    End Function
    Public Sub setRotation(t As Double)
        _rotation = t
        setAxes(t)
    End Sub
    Public Function getCornerParticles() As ArrayList
        If _cornerPositions.Count() = 0 Then
            getCornerPositions()
        End If
        If _cornerParticles.Count() = 0 Then
            Dim cp1 As New CircleParticle(0, 0, 1, False, 1, 0.3, _
             0)
            cp1.setCollidable(False)
            cp1.setVisible(False)
            APEngine.addParticle(cp1)
            Dim cp2 As New CircleParticle(0, 0, 1, False, 1, 0.3, _
             0)
            cp2.setCollidable(False)
            cp2.setVisible(False)
            APEngine.addParticle(cp2)
            Dim cp3 As New CircleParticle(0, 0, 1, False, 1, 0.3, _
             0)
            cp3.setCollidable(False)
            cp3.setVisible(False)
            APEngine.addParticle(cp3)
            Dim cp4 As New CircleParticle(0, 0, 1, False, 1, 0.3, _
             0)
            cp4.setCollidable(False)
            cp4.setVisible(False)
            APEngine.addParticle(cp4)
            _cornerParticles.Add(cp1)
            _cornerParticles.Add(cp2)
            _cornerParticles.Add(cp3)
            _cornerParticles.Add(cp4)
            updateCornerParticles()
        End If
        Return _cornerParticles
    End Function
    Public Function getCornerPositions() As ArrayList
        If _cornerPositions.Count() = 0 Then
            _cornerPositions.Add(New Vector(0, 0))
            _cornerPositions.Add(New Vector(0, 0))
            _cornerPositions.Add(New Vector(0, 0))
            _cornerPositions.Add(New Vector(0, 0))
            updateCornerPositions()
        End If
        Return _cornerPositions
    End Function
    Public Overridable Sub paint()
        If dc Is Nothing Then
            dc = getDefaultContainer()
        End If
        If Not getVisible() Then
            Return
            Exit Sub
        End If
        Dim j As Integer = 0
        For j = 0 To 3
            Dim i As Integer = j
            Dim X1 As Double = (DirectCast(_cornerPositions.Item(i), Vector)).x
            Dim Y1 As Double = (DirectCast(_cornerPositions.Item(i), Vector)).y
            If j = 3 Then i = -1

            Dim X2 As Single = (DirectCast(_cornerPositions.Item(i + 1), Vector)).x
            Dim Y2 As Single = (DirectCast(_cornerPositions.Item(i + 1), Vector)).y
            ' Dim line As Line2D = New Line2D.Double(X1, Y1, X2, Y2)
            'dc.draw(line)
            dc.DrawLine(New Pen(Color.Black), New PointF(X1, Y1), New PointF(X2, Y2))
            '  System.Math.Max(System.Threading.Interlocked.Increment(j), j - 1)
            ' Debug.Print(X1, Y1, X2, Y2)
        Next
    End Sub
    Public Overrides Sub update(dt2 As Double)
        MyBase.update(dt2)
        If _cornerPositions.Count() <> 0 Then
            updateCornerPositions()
        End If
        If _cornerParticles.Count() <> 0 Then
            updateCornerParticles()
        End If
    End Sub
    Public Function getAxes() As ArrayList
        Return _axes
    End Function
    Public Function getExtents() As ArrayList
        Return _extents
    End Function
    Public Overrides Function getProjection(axis As Vector) As Interval
        Dim radius As Double
        radius = (DirectCast(_extents.Item(0), Double)) * Math.Abs(axis.dot(DirectCast(_axes.Item(0), Vector))) + (DirectCast(_extents.Item(1), Double)) * Math.Abs(axis.dot(DirectCast(_axes.Item(1), Vector)))
        Dim c As Double = curr.dot(axis)
        interval.min = c - radius
        interval.max = c + radius
        Return interval
    End Function
    Public Sub updateCornerPositions()
        Dim ae0_x As Double = (DirectCast(_axes.Item(0), Vector)).x * (DirectCast(_extents.Item(0), Double))
        Dim ae0_y As Double = (DirectCast(_axes.Item(0), Vector)).y * (DirectCast(_extents.Item(0), Double))
        Dim ae1_x As Double = (DirectCast(_axes.Item(1), Vector)).x * (DirectCast(_extents.Item(1), Double))
        Dim ae1_y As Double = (DirectCast(_axes.Item(1), Vector)).y * (DirectCast(_extents.Item(1), Double))
        Dim emx As Double = ae0_x - ae1_x
        Dim emy As Double = ae0_y - ae1_y
        Dim epx As Double = ae0_x + ae1_x
        Dim epy As Double = ae0_y + ae1_y
        Dim cornerPosition1 As New Vector(0, 0)
        Dim cornerPosition2 As New Vector(0, 0)
        Dim cornerPosition3 As New Vector(0, 0)
        Dim cornerPosition4 As New Vector(0, 0)
        cornerPosition1.x = curr.x - epx
        cornerPosition1.y = curr.y - epy
        _cornerPositions.Item(0) = cornerPosition1
        cornerPosition2.x = curr.x + emx
        cornerPosition2.y = curr.y + emy
        _cornerPositions.Item(1) = cornerPosition2
        cornerPosition3.x = curr.x + epx
        cornerPosition3.y = curr.y + epy
        _cornerPositions.Item(2) = cornerPosition3
        cornerPosition4.x = curr.x - emx
        cornerPosition4.y = curr.y - emy
        _cornerPositions.Item(3) = cornerPosition4
    End Sub
    Private Sub updateCornerParticles()
        Dim i As Integer = 0
        For i = 0 To 3
            DirectCast(getCornerParticles().Item(i), AbstractParticle).setpx((DirectCast(_cornerPositions.Item(i), Vector)).x)
            DirectCast(getCornerParticles().Item(i), AbstractParticle).setpy((DirectCast(_cornerPositions.Item(i), Vector)).y)
            'System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
        Next
    End Sub
    Private Sub setAxes(t As Double)
        Dim s As Double = Math.Sin(t)
        Dim c As Double = Math.Cos(t)
        DirectCast(_axes.Item(0), Vector).x = c
        DirectCast(_axes.Item(0), Vector).y = s
        DirectCast(_axes.Item(1), Vector).x = -s
        DirectCast(_axes.Item(1), Vector).y = c
    End Sub
End Class


