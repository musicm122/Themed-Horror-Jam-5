extends Node2D

signal player_interacting
signal player_interacting_complete
var playerRef
export var timeline = ""
var interactAction = "interact"
var canInteract = false
var shouldRemove = false

func waitSec(sec):
	var t = Timer.new()
	t.set_wait_time(sec)
	t.set_one_shot(true)
	self.add_child(t)
	t.start()
	yield(t, "timeout")
	t.queue_free()

func IsInteracting():
	return Input.is_action_just_pressed(interactAction)

func onInteract():	
	get_tree().call_group("Movable", "OnExaminablePlayerInteracting")
	emit_signal("player_interacting")
	canInteract = false
	var dialog = Dialogic.start(timeline) 
	dialog.connect("dialogic_signal", self, "dialog_listener")
	add_child(dialog)

func removeItem():
	get_tree().paused = false
	visible = false
	set_process(false)
	set_physics_process(false)
	set_process_input(false)
	if(playerRef!=null):
		playerRef.HideExamineNotification()
		
	print("printing children")
	for _i in self.get_children():
		print(_i)
		
	if(get_child_count()>0):
		var area = get_child(0)
		#var area = get_node("Area2D")
		
		if(area!=null && area.name == "Area2D"):
			area.disconnect("body_entered", self, "_on_Area2D2_body_entered")
			area.disconnect("body_exited", self, "_on_Area2D2_body_exited")
			remove_child(area)

func dialog_listener(arg):
	get_tree().paused = true
	print("dialog_listener called with ", arg)

	match arg:
		"PizzaGiven":
			get_tree().call_group("Player", "RemoveItem", "pizza")
		"Pizza":
			if name == "Item_Pizza" :
				get_tree().call_group("Player", "AddItem", "pizza", 1)
				shouldRemove = true
		"Flashlight":
			if name == "Flashlight" :
				get_tree().call_group("Player", "AddItem", "Flashlight", 1)
				shouldRemove = true
		"Keys":
			if name == "Key" :
				get_tree().call_group("Player", "AddItem", "KeyA", 1)
				shouldRemove = true
				

	print("ending dialog")
	get_tree().call_group("Movable", "OnExaminablePlayerInteractingComplete")
	emit_signal("player_interacting_complete")
	print("player should be able to move")
	yield(get_tree().create_timer(0.2), "timeout")
	get_tree().paused = false
	canInteract = true
	if(shouldRemove) :
		removeItem()
			
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	if canInteract && IsInteracting():
		onInteract()

func _on_Area2D2_body_entered(body):
	print("entered area")
	canInteract = body.get_name() == "Player"
	playerRef= body
	body.ShowExamineNotification()

func _on_Area2D2_body_exited(body):
	print("exited area")
	if body.get_name() == "Player": 
		playerRef = body
		print("Player exited area")
		canInteract = false
		body.HideExamineNotification()	
