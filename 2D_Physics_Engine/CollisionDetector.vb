Option Strict Off
Public Class CollisionDetector
    Public Shared Sub test(objA As AbstractParticle, objB As AbstractParticle)
        If objA.getFixed() AndAlso objB.getFixed() Then
            Return
            Exit Sub
        End If
        '// rectangle to rectangle
        If TypeOf objA Is RectangleParticle And TypeOf objB Is RectangleParticle Then
            testOBBvsOBB(objA, objB)
            '// circle to circle
        ElseIf TypeOf objA Is CircleParticle And TypeOf objB Is CircleParticle Then
            testCirclevsCircle(objA, objB)
            '// rectangle to circle - two ways
        ElseIf TypeOf objA Is RectangleParticle And TypeOf objB Is CircleParticle Then
            testOBBvsCircle(objA, objB)
        ElseIf TypeOf objA Is CircleParticle And TypeOf objB Is RectangleParticle Then
            testOBBvsCircle(objB, objA)
        End If
        'testOBBvsOBB(DirectCast(objA, RectangleParticle), DirectCast(objB, RectangleParticle))
    End Sub

    Private Shared Sub testOBBvsOBB(ra As RectangleParticle, rb As RectangleParticle)
        Dim collisionNormal As New Vector(0, 0)
        Dim collisionDepth As Double = POSITIVE_INFINITY
        Dim i As Integer = 0
        For i = 0 To 1
            Dim axisA As Vector = DirectCast(ra.getAxes().Item(i), Vector)
            Dim depthA As Double = testIntervals(ra.getProjection(axisA), rb.getProjection(axisA))
            If depthA = 0 Then
                Return
                Exit Sub
            End If
            Dim axisB As Vector = DirectCast(rb.getAxes().Item(i), Vector)
            Dim depthB As Double = testIntervals(ra.getProjection(axisB), rb.getProjection(axisB))
            If depthB = 0 Then
                Return
                Exit Sub
            End If
            Dim absA As Double = Math.Abs(depthA)
            Dim absB As Double = Math.Abs(depthB)
            If absA < Math.Abs(collisionDepth) OrElse absB < Math.Abs(collisionDepth) Then
                Dim altb As Boolean = absA < absB
                collisionNormal = If(altb, axisA, axisB)
                collisionDepth = If(altb, depthA, depthB)
            End If
        Next
        'System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
        CollisionResolver.resolveParticleParticle(ra, rb, collisionNormal, collisionDepth)
    End Sub
    Private Shared Sub testOBBvsCircle(ra As RectangleParticle, ca As CircleParticle)
        Dim collisionNormal As New Vector(0, 0)
        Dim collisionDepth As Double = POSITIVE_INFINITY
        Dim depths As New ArrayList(2)
        Dim i As Integer = 0
        'While i < 2
        For i = 0 To 1
            Dim boxAxis As Vector = DirectCast(ra.getAxes().Item(i), Vector)
            Dim depth As Double = testIntervals(ra.getProjection(boxAxis), ca.getProjection(boxAxis))
            If depth = 0 Then
                Return
                Exit Sub
            End If
            If Math.Abs(depth) < Math.Abs(collisionDepth) Then
                collisionNormal = boxAxis
                collisionDepth = depth
            End If
            depths.Insert(i, depth)
            ' System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
        Next
        'End While
        Dim r As Double = ca.getRadius()
        If Math.Abs(depths.Item(0)) < r AndAlso Math.Abs(depths.Item(1)) < r Then
            Dim vertex As Vector = closestVertexOnOBB(ca.curr, ra)
            collisionNormal = vertex.minus(ca.curr)
            Dim mag As Double = collisionNormal.magnitude()
            collisionDepth = r - mag
            If collisionDepth > 0 Then
                collisionNormal.divEquals(mag)
            Else
                Return
                Exit Sub
            End If
        End If
        CollisionResolver.resolveParticleParticle(ra, ca, collisionNormal, collisionDepth)
    End Sub
    Private Shared Sub testCirclevsCircle(ca As CircleParticle, cb As CircleParticle)
        Dim depthX As Double = testIntervals(ca.getIntervalX(), cb.getIntervalX())
        If depthX = 0 Then
            Return
            Exit Sub
        End If
        Dim depthY As Double = testIntervals(ca.getIntervalY(), cb.getIntervalY())
        If depthY = 0 Then
            Return
            Exit Sub
        End If
        Dim collisionNormal As Vector = ca.curr.minus(cb.curr)
        Dim mag As Double = collisionNormal.magnitude()
        Dim collisionDepth As Double = (ca.getRadius() + cb.getRadius()) - mag
        If collisionDepth > 0 Then
            collisionNormal.divEquals(mag)
            CollisionResolver.resolveParticleParticle(ca, cb, collisionNormal, collisionDepth)
        End If
    End Sub
    Private Shared Function testIntervals(intervalA As Interval, intervalB As Interval) As Double
        If intervalA.max < intervalB.min Then
            Return 0
            Exit Function
        End If
        If intervalB.max < intervalA.min Then
            Return 0
            Exit Function
        End If
        Dim lenA As Double = intervalB.max - intervalA.min
        Dim lenB As Double = intervalB.min - intervalA.max
        Return If((Math.Abs(lenA) < Math.Abs(lenB)), lenA, lenB)
    End Function
    Private Shared Function closestVertexOnOBB(p As Vector, r As RectangleParticle) As Vector
        Dim d As Vector = p.minus(r.curr)
        Dim q As New Vector(r.curr.x, r.curr.y)
        Dim i As Integer = 0
        For i = 0 To 1
            Dim dist As Double = d.dot(DirectCast(r.getAxes().Item(i), Vector))
            If dist >= 0 Then
                dist = (DirectCast(r.getExtents().Item(i), Double))
            ElseIf dist < 0 Then
                dist = -(DirectCast(r.getExtents().Item(i), Double))
            End If
            q.plusEquals((DirectCast(r.getAxes().Item(i), Vector)).mult(dist))
            '  System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
        Next
        Return q
    End Function
End Class


