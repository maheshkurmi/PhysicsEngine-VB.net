Option Strict Off

Public Class CircleParticle
    Inherits AbstractParticle
    Private _radius As Double

    Public Sub New(x As Double, y As Double, radius As Double, fixed As Boolean, mass As Double, elasticity As Double, _
     friction As Double)
        MyBase.new(x, y, fixed, mass, elasticity, friction)
        _radius = radius
    End Sub
    Public Function getRadius() As Double
        Return _radius
    End Function
    Public Sub setRadius(r As Double)
        _radius = r
    End Sub
    Public Overridable Sub paint()
        If dc Is Nothing Then
            dc = getDefaultContainer()
        End If

        If Not getVisible() Then
            Return
            Exit Sub
        End If

        'Dim circle As New Ellipse2D.Double(curr.x - getRadius(), curr.y - getRadius(), DirectCast(getRadius(), Double) * 2, DirectCast(getRadius(), Double) * 2)
        dc.DrawEllipse(New Pen(Color.Black), CSng(curr.x - getRadius()), CSng(curr.y - getRadius()), CSng(getRadius()) * 2, CSng(getRadius()) * 2)
        ' dc.draw(circle)
    End Sub
    Public Overrides Function getProjection(axis As Vector) As Interval
        Dim c As Double = curr.dot(axis)
        interval.min = c - _radius
        interval.max = c + _radius
        Return interval
    End Function
    Public Function getIntervalX() As Interval
        interval.min = curr.x - _radius
        interval.max = curr.x + _radius
        Return interval
    End Function
    Public Function getIntervalY() As Interval
        interval.min = curr.y - _radius
        interval.max = curr.y + _radius
        Return interval
    End Function
End Class


