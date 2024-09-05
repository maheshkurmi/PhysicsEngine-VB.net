Option Strict Off
Public Class SpringConstraint
    Inherits AbstractConstraint
    Private p1 As AbstractParticle
    Private p2 As AbstractParticle
    Private restLen As Double
    Private delta As Vector
    Private deltaLength As Double
    Private _collisionRectWidth As Double
    Private _collisionRectScale As Double
    Private _collidable As Boolean
    Private collisionRect As SpringConstraintParticle
    Public Sub New(p1 As AbstractParticle, p2 As AbstractParticle, stiffness As Double)
        MyBase.new(stiffness)
        Me.p1 = p1
        Me.p2 = p2
        checkParticlesLocation()
        _collisionRectWidth = 1
        _collisionRectScale = 1
        _collidable = False
        delta = p1.curr.minus(p2.curr)
        deltaLength = p1.curr.distance(p2.curr)
        restLen = deltaLength
    End Sub
    Public Function getRotation() As Double
        Return Math.Atan2(delta.y, delta.x)
    End Function
    Public Function getCenter() As Vector
        Return (p1.curr.plus(p2.curr)).divEquals(2)
    End Function
    Public Function getCollisionRectScale() As Double
        Return _collisionRectScale
    End Function
    Public Sub setCollisionRectScale(scale As Double)
        _collisionRectScale = scale
    End Sub
    Public Function getCollisionRectWidth() As Double
        Return _collisionRectWidth
    End Function
    Public Sub setCollisionRectWidth(w As Double)
        _collisionRectWidth = w
    End Sub
    Public Function getRestLength() As Double
        Return restLen
    End Function
    Public Sub setRestLength(r As Double)
        restLen = r
    End Sub
    Public Function getCollidable() As Boolean
        Return _collidable
    End Function
    Public Sub setCollidable(b As Boolean)
        _collidable = b
        If _collidable Then
            collisionRect = New SpringConstraintParticle(p1, p2)
            orientCollisionRectangle()
        Else
            collisionRect = Nothing
        End If
    End Sub
    Public Function isConnectedTo(p As AbstractParticle) As Boolean
        Return (p.Equals(p1) Or p.Equals(p2))
    End Function
    Public Sub paint()
        If dc Is Nothing Then
            dc = getDefaultContainer()
        End If
        If _collidable Then
            collisionRect.paint()
        Else
            If Not getVisible() Then
                Return
                Exit Sub
            End If
            Dim X1 As Double = p1.curr.x
            Dim Y1 As Double = p1.curr.y
            Dim X2 As Double = p2.curr.x
            Dim Y2 As Double = p2.curr.y
            'Dim line As Line2D = New Line2D.Double(X1, Y1, X2, Y2)
            'dc.draw(line)
            dc.DrawLine(New Pen(Color.Black), New Point(X1, Y1), New Point(X2, Y2))
        End If
    End Sub
    Public Overrides Sub resolve()
        If p1.getFixed() And p2.getFixed() Then
            Return
            Exit Sub
        End If
        delta = p1.curr.minus(p2.curr)
        deltaLength = p1.curr.distance(p2.curr)
        If _collidable Then
            orientCollisionRectangle()
        End If
        Dim diff As Double = (deltaLength - restLen) / deltaLength
        Dim dmd As Vector = delta.mult(diff * MyBase.getStiffness())
        Dim invM1 As Double = p1.getInvMass()
        Dim invM2 As Double = p2.getInvMass()
        Dim sumInvMass As Double = invM1 + invM2
        If Not p1.getFixed() Then
            p1.curr.minusEquals(dmd.mult(invM1 / sumInvMass))
        End If
        If Not p2.getFixed() Then
            p2.curr.plusEquals(dmd.mult(invM2 / sumInvMass))
        End If
    End Sub
    Public Function getCollisionRect() As RectangleParticle
        Return collisionRect
    End Function
    Private Sub orientCollisionRectangle()
        Dim c As Vector = getCenter()
        Dim rot As Double = getRotation()
        collisionRect.curr.setTo(c.x, c.y)
        collisionRect.getExtents.Item(0) = ((deltaLength / 2) * _collisionRectScale)
        collisionRect.getExtents.Item(1) = ((_collisionRectWidth / 2))
        collisionRect.setRotation(rot)
    End Sub
    Private Sub checkParticlesLocation()
        If p1.curr.x = p2.curr.x AndAlso p1.curr.y = p2.curr.y Then
            Throw New Error1 '("The two particles specified for a SpringContraint can't be at the same location")
        End If
    End Sub
End Class


