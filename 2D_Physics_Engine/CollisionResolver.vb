Public Class CollisionResolver
    Public Shared Sub resolveParticleParticle(pa As AbstractParticle, pb As AbstractParticle, normal As Vector, depth As Double)

        Dim mtd As Vector = normal.mult(depth)
        Dim te As Double = pa.getElasticity() + pb.getElasticity()
        Dim tf As Double = 1 - (pa.getFriction() + pb.getFriction())
        If tf > 1 Then
            tf = 1
        ElseIf tf < 0 Then
            tf = 0
        End If
        Dim ma As Double = If((pa.getFixed()), 100000, pa.getMass())
        Dim mb As Double = If((pb.getFixed()), 100000, pb.getMass())
        Dim tm As Double = ma + mb
        Dim ca As Collision = pa.getComponents(normal)
        Dim cb As Collision = pb.getComponents(normal)
        If ca.vn.x > 5 Then
            Dim i As Integer = 9
        End If
        Dim vnA As Vector = (cb.vn.mult((te + 1) * mb).plus(ca.vn.mult(ma - te * mb))).divEquals(tm)
        Dim vnB As Vector = (ca.vn.mult((te + 1) * ma).plus(cb.vn.mult(mb - te * ma))).divEquals(tm)
        ca.vt.multEquals(tf)
        cb.vt.multEquals(tf)
        Dim mtdA As Vector = mtd.mult(mb / tm)
        Dim mtdB As Vector = mtd.mult(-ma / tm)
        If Not pa.getFixed() Then
            pa.resolveCollision(mtdA, vnA.plusEquals(ca.vt), normal, depth, -1)
            If vnA.x > 5 Then
                Dim i As Integer = 9
            End If
        End If
        If Not pb.getFixed() Then
            pb.resolveCollision(mtdB, vnB.plusEquals(cb.vt), normal, depth, 1)
        End If
    End Sub
End Class


