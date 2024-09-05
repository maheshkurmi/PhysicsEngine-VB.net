Option Strict Off
Imports System.Threading


Public Class frmDemo
   

    Private usedTime As Long

    Private APEngine As APEngine = New APEngine()
    Private paintQueue As ArrayList

    Private t As Integer
    Private g As Graphics
    Private wheelParticleA As WheelParticle
    Private wheelParticleB As WheelParticle
    Private rotatingRect As RectangleParticle
    Private Sub frmDemo_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Dim keySpeed As Double = 0.2
        If (e.KeyCode = Keys.Left) Then
            wheelParticleA.setAngularVelocity(-keySpeed)
            wheelParticleB.setAngularVelocity(-keySpeed)
            ' updateWorld()
            '  paintWorld()
            '  Me.Invalidate()
        ElseIf (e.KeyCode = Keys.Right) Then
            wheelParticleA.setAngularVelocity(keySpeed)
            wheelParticleB.setAngularVelocity(keySpeed)
            ' updateWorld()
            ' paintWorld()
            ' Me.Invalidate()

        ElseIf (e.KeyCode = Keys.Up) Then
            timerGame.Enabled = False
            updateWorld()
            paintWorld()
            Me.Invalidate()
        End If
    End Sub
    '
    Private Sub frmDemo_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        wheelParticleA.setAngularVelocity(0)
        wheelParticleB.setAngularVelocity(0)
        timerGame.Enabled = True
    End Sub

    Private Sub frmDemo_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        'Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        Me.DoubleBuffered = True
        Me.SetStyle(
        ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        Me.UpdateStyles()
        g = Me.CreateGraphics
        ' g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        APEngine.setDefaultContainer(g)
        initWorld()
        ' Thread.Sleep(10)
        timerGame.Enabled = True
        'game()
        ' th.Start()
    End Sub

    Public Sub initWorld()
        ' set up the events, main loop handler, and the engine. you don't have to use
        ' enterframe. you just need to call the ApeEngine.step() method wherever
        ' and however your handling your program cycle.
        ' the argument here is the deltaTime value. Higher values result in faster simulations.
        APEngine.init(1 / 3, Me.Width, Me.Height)
        ' SELECTIVE is better for dealing with lots of little particles colliding, 
        ' as in the little rects and circles in this example
        APEngine.setCollisionResponseMode(APEngine.SELECTIVE)
        ' gravity -- particles of varying masses are affected the same
        APEngine.addMasslessForce(New Vector(0, 3))
        'APEngine.addMasslessForce(new Vector(-3,0));
        ' surfaces
        Dim floor As RectangleParticle = New RectangleParticle(325, 324, 649, 50, 0, True, 1, 0.3, 0)
        APEngine.addParticle(floor)
        Dim floorBumpA As RectangleParticle = New RectangleParticle(400, 295, 90, 30, 0.4, True, 1, 0.3, 0)
        APEngine.addParticle(floorBumpA)
        Dim floorBumpB As RectangleParticle = New RectangleParticle(330, 295, 90, 30, -0.4, True, 1, 0.3, 0)
        APEngine.addParticle(floorBumpB)
        Dim floorLeftAngle As RectangleParticle = New RectangleParticle(80, 290, 120, 20, 0.5, True, 1, 0.3, 0)
        APEngine.addParticle(floorLeftAngle)
        Dim leftWall As RectangleParticle = New RectangleParticle(15, 99, 30, 500, 0, True, 1, 0.3, 0)
        APEngine.addParticle(leftWall)
        Dim rightWall As RectangleParticle = New RectangleParticle(634, 99, 30, 500, 0, True, 1, 0.3, 0)
        APEngine.addParticle(rightWall)
        Dim bridgeStart As RectangleParticle = New RectangleParticle(80, 70, 150, 25, 0, True, 1, 0.3, 0)
        APEngine.addParticle(bridgeStart)
        Dim bridgeEnd As RectangleParticle = New RectangleParticle(380, 70, 100, 25, 0, True, 1, 0.3, 0)
        APEngine.addParticle(bridgeEnd)
        Dim bridgeEndAngle As RectangleParticle = New RectangleParticle(455, 102, 100, 25, 0.8, True, 1, 0.3, 0)
        APEngine.addParticle(bridgeEndAngle)
        Dim rightWallAngle As RectangleParticle = New RectangleParticle(595, 102, 100, 25, -0.8, True, 1, 0.3, 0)
        APEngine.addParticle(rightWallAngle)
        ' rotator
        rotatingRect = New RectangleParticle(525, 180, 70, 14, 0, True, 1, 0.3, 0)
        APEngine.addParticle(rotatingRect)
        Dim littleRect As RectangleParticle = New RectangleParticle(545, 238, 10, 10, 0, False, 1, 0.3, 0)
        APEngine.addParticle(littleRect)
        'SpringConstraint rotConnector = new SpringConstraint((AbstractParticle)rotatingRect.getCornerParticles().get(1), (AbstractParticle)littleRect, 0.2);
        '    APEngine.addConstraint(rotConnector);
        ' bridge
        Dim bridgePA As CircleParticle = New CircleParticle(200, 70, 4, False, 1, 0.3, 0)
        APEngine.addParticle(bridgePA)
        Dim bridgePB As CircleParticle = New CircleParticle(240, 70, 4, False, 1, 0.3, 0)
        APEngine.addParticle(bridgePB)
        Dim bridgePC As CircleParticle = New CircleParticle(280, 70, 4, False, 1, 0.3, 0)
        APEngine.addParticle(bridgePC)
        Dim bridgeConnA As SpringConstraint = New SpringConstraint(CType(bridgeStart.getCornerParticles.Item(1), AbstractParticle), bridgePA, 0.9)
        bridgeConnA.setCollidable(True)
        bridgeConnA.setCollisionRectWidth(10)
        bridgeConnA.setCollisionRectScale(0.6)
        APEngine.addConstraint(bridgeConnA)
        Dim bridgeConnB As SpringConstraint = New SpringConstraint(bridgePA, bridgePB, 0.9)
        bridgeConnB.setCollidable(True)
        bridgeConnB.setCollisionRectWidth(2)
        bridgeConnB.setCollisionRectScale(0.8)
        APEngine.addConstraint(bridgeConnB)
        Dim bridgeConnC As SpringConstraint = New SpringConstraint(bridgePB, bridgePC, 0.9)
        bridgeConnC.setCollidable(True)
        bridgeConnC.setCollisionRectWidth(10)
        bridgeConnC.setCollisionRectScale(0.6)
        APEngine.addConstraint(bridgeConnC)
        Dim bridgeConnD As SpringConstraint = New SpringConstraint(bridgePC, CType(bridgeEnd.getCornerParticles.Item(0), AbstractParticle), 0.9)
        bridgeConnD.setCollidable(True)
        bridgeConnD.setCollisionRectWidth(10)
        bridgeConnD.setCollisionRectScale(0.6)
        APEngine.addConstraint(bridgeConnD)
        ' car 
        wheelParticleA = New WheelParticle(60, 10, 20, False, 1, 0.3, 0, 1)
        wheelParticleA.setMass(2)
        APEngine.addParticle(wheelParticleA)
        wheelParticleB = New WheelParticle(140, 10, 20, False, 1, 0.3, 0, 1)
        wheelParticleB.setMass(2)
        APEngine.addParticle(wheelParticleB)

        Dim rotConnector1 As SpringConstraint = New SpringConstraint(CType(rotatingRect.getCornerParticles.Item(1), AbstractParticle), CType(littleRect, AbstractParticle), 0.2)
        APEngine.addConstraint(rotConnector1)
        Dim wheelConnector As SpringConstraint = New SpringConstraint(wheelParticleA, wheelParticleB, 0.5)
        wheelConnector.setCollidable(True)
        wheelConnector.setCollisionRectWidth(CType(10, Double))
        wheelConnector.setCollisionRectScale(CType(0.9, Double))
        APEngine.addConstraint(wheelConnector)
        ' little boxes
        Dim i As Integer = 0
        Do While (i < 20)
            Dim px As Double = ((7 * i) _
                        + 120)
            APEngine.addParticle(New RectangleParticle(px, 200, 10, 5, (Rnd() * Math.PI), False, 1.8, 0.1, 0))
            APEngine.addParticle(New CircleParticle((px + 40), 210, 3.5, False, 1.8, 0.1, 0))
            i = (i + 1)
        Loop
        paintQueue = APEngine.getAll
    End Sub
    Public Sub initWorldgg()
        ' set up the events, main loop handler, and the engine. you don't have to use
        ''/ enterframe. you just need to call the ApeEngine.step() method wherever
        ' and however your handling your program cycle.
        'the argument here is the deltaTime value. Higher values result in faster simulations.
        APEngine.init(1D / 3.0, Me.Width, Me.Height)
        '	// SELECTIVE is better for dealing with lots of little particles colliding, 
        '	// as in the little rects and circles in this example
        APEngine.setCollisionResponseMode(APEngine.STANDARD)
        '	// gravity -- particles of varying masses are affected the same
        APEngine.addMasslessForce(New Vector(0, 3))
        'APEngine.addMasslessForce(New Vector(3, 0))
        '	// surfaces
        Dim floor As RectangleParticle
        floor = New RectangleParticle(300, 324, 600, 50, 0, True, 1, 0.3, 0)
        APEngine.addParticle(floor)

        Dim floorleftAngle As RectangleParticle
        floorleftAngle = New RectangleParticle(80, 290, 120, 20, 0.5, True, 1, 0.3, 0)
        APEngine.addParticle(floorleftAngle)
        Dim leftwall As RectangleParticle
        leftwall = New RectangleParticle(15, 99, 30, 500, 0, True, 1, 0.3, 0)
        APEngine.addParticle(leftwall)

        Dim rightWall As RectangleParticle
        rightWall = New RectangleParticle(634, 99, 30, 500, 0, True, 1, 0.3, 0)
        APEngine.addParticle(rightWall)


        '	// car 
        wheelParticleA = New WheelParticle(60, 40, 20, False, 1, 0.3, 0, 1)
        wheelParticleA.setMass(2)
        APEngine.addParticle(wheelParticleA)

        wheelParticleB = New WheelParticle(140, 40, 20, False, 1, 0.3, 0, 1)
        wheelParticleB.setMass(2)
        APEngine.addParticle(wheelParticleB)

        Dim wheelConnector As SpringConstraint
        wheelConnector = New SpringConstraint(wheelParticleA, wheelParticleB, 0.1)
        wheelConnector.setCollidable(True)
        wheelConnector.setCollisionRectWidth(10.0)
        wheelConnector.setCollisionRectScale(0.9D)
        APEngine.addConstraint(wheelConnector)

        paintQueue = APEngine.getAll()
    End Sub

    Public Sub updateWorld()
        APEngine.StepUp()
        If Not rotatingRect Is Nothing Then rotatingRect.setRotation(rotatingRect.getRotation() + 0.03)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        ' paintWorld()
        ' g = Me.CreateGraphics
        '  g.Clear(Color.White)
        ' g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        '    // TG set the default container.
        ' e.Graphics.FromImage(APEngine.bmp)
        ' e.Graphics.Clear(Me.BackColor)
        e.Graphics.DrawImage(APEngine.bmp, New Point(5, 10))
    End Sub
    Public Sub paintWorld()
        For i As Integer = 0 To paintQueue.Count() - 1
            ''//TG TODO need to write code that determined the type of objects and sets their method. 
            ' paintQueue.Item(i).paint()
            If (TypeOf paintQueue.Item(i) Is RectangleParticle) Then
                DirectCast(paintQueue.Item(i), RectangleParticle).paint()
            ElseIf TypeOf paintQueue.Item(i) Is CircleParticle Then
                ' DirectCast(paintQueue.Item(i), CircleParticle).paint()
                paintQueue.Item(i).paint()
                With DirectCast(paintQueue.Item(i), CircleParticle)
                    Dim r As Single
                    r = .getRadius
                    '  e.Graphics.DrawEllipse(New Pen(Color.Red), CSng(.curr.x), CSng(.curr.y), r, r)
                End With
            ElseIf (TypeOf paintQueue.Item(i) Is SpringConstraint) Then
                DirectCast(paintQueue.Item(i), SpringConstraint).paint()
                paintQueue.Item(i).paint()
            Else
                'paintQueue.Item(i).paint()
            End If
        Next
        ' paintfps(g)
        'strategy.show()
        '//TG TODO not sure if I should be clearing the screen, otherwise the screen does not refesh properly, need to investigate the best approach.
        'g.clearRect(0,0,Stage.SCREEN_WIDTH,Stage.SCREEN_HEIGHT);
    End Sub

    Public Sub paintfps(g As Graphics)
        '  g.setFont( new Font("Arial",Font.BOLD,12));
        Dim font1 As Font
        font1 = New Font("Areal", 10)
        If (usedTime > 0) Then
            g.DrawString(CStr((1000 / usedTime)) & "fps" & CStr(SCREEN_WIDTH - 50 + PLAY_HEIGHT), Font, New SolidBrush(Color.Red), New Point(10, 20))
        Else
            g.DrawString("--- fps" & SCREEN_WIDTH - 50 & PLAY_HEIGHT, Font, New SolidBrush(Color.Red), New Point(10, 20))
        End If
    End Sub

    Private Sub timerGame_Tick(sender As System.Object, e As System.EventArgs) Handles timerGame.Tick
        updateWorld()
        paintWorld()
        '  Me.BackgroundImage = New Bitmap(600, 300)
        '  Me.BackgroundImage = APEngine.bmp
        Me.Invalidate()
    End Sub

    
End Class