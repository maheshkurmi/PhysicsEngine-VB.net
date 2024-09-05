Option Strict Off
Public Class RimParticle
    Public curr As Vector
    Public prev As Vector
    Private wr As Double
    Private av As Double
    Private sp As Double
    Private maxTorque As Double
    Public Sub New(r As Double, mt As Double)
        curr = New Vector(r, 0)
        prev = New Vector(0, 0)
        sp = 0
        av = 0
        maxTorque = mt
        wr = r
    End Sub
    Public Function getSpeed() As Double
        Return sp
    End Function
    Public Sub setSpeed(s As Double)
        sp = s
    End Sub
    Public Function getAngularVelocity() As Double
        Return av
    End Function
    Public Sub setAngularVelocity(s As Double)
        av = s
    End Sub
    Public Sub update(dt As Double)
        sp = Math.Max(-maxTorque, Math.Min(maxTorque, sp + av))
        Dim dx As Double = -curr.y
        Dim dy As Double = curr.x
        Dim len As Double = Math.Sqrt(dx * dx + dy * dy)
        dx = dx / len
        dy = dy / len
        curr.x = curr.x + sp * dx
        curr.y = curr.y + sp * dy
        Dim ox As Double = prev.x
        Dim oy As Double = prev.y
        prev.x = curr.x
        Dim px As Double = prev.x '= curr.x
        prev.y = curr.y
        Dim py As Double = prev.y '= curr.y
        curr.x = curr.x + APEngine.getDamping() * (px - ox)
        curr.y = curr.y + APEngine.getDamping() * (py - oy)
        Dim clen As Double = Math.Sqrt(curr.x * curr.x + curr.y * curr.y)
        Dim diff As Double = (clen - wr) / clen
        curr.x = curr.x - curr.x * diff
        curr.y = curr.y - curr.y * diff
    End Sub
End Class


