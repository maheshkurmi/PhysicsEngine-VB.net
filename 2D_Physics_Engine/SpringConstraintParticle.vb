Public Class SpringConstraintParticle
    Inherits RectangleParticle
    Private p1 As AbstractParticle
    Private p2 As AbstractParticle
    Private avgVelocity As Vector
    Public Sub New(p1 As AbstractParticle, p2 As AbstractParticle)
        MyBase.new(0, 0, 0, 0, 0, False, _
         1, 0.3, 0)
        Me.p1 = p1
        Me.p2 = p2
        avgVelocity = New Vector(0, 0)
    End Sub
    Public Overrides Function getMass() As Double
        Return (p1.getMass() + p2.getMass()) / 2
    End Function
    Public Overrides Function getVelocity() As Vector
        Dim p1v As Vector = p1.getVelocity()
        Dim p2v As Vector = p2.getVelocity()
        avgVelocity.setTo(((p1v.x + p2v.x) / 2), ((p1v.y + p2v.y) / 2))
        Return avgVelocity
    End Function
    Public Overrides Sub paint()
        If Not _cornerPositions Is Nothing Then
            updateCornerPositions()
        End If
        MyBase.paint()
    End Sub
    Public Overrides Sub resolveCollision(mtd As Vector, vel As Vector, n As Vector, d As Double, o As Double)
        If Not p1.getFixed() Then
            p1.curr.plusEquals(mtd)
            p1.setVelocity(vel)
        End If
        If Not p2.getFixed() Then
            p2.curr.plusEquals(mtd)
            p2.setVelocity(vel)
        End If
    End Sub
End Class


