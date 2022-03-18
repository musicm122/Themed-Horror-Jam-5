extends Node2D

signal player_interacting
signal player_interacting_complete

export var timeline = ""
var interactAction = "interact"
var canInteract = false

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

func dialog_listener(arg):
	get_tree().paused = true
	print("dialog_listener called with ", arg)

	match arg:
		"PizzaGiven":
			get_tree().call_group("Player", "RemoveItem", "pizza")
		"Pizza":
			if name == "Item_Pizza" :
				get_tree().call_group("Player", "AddItem", "pizza", 1)
				removeItem()
		"Flashlight":
			if name == "Item_Flashlight" :
				get_tree().call_group("Player", "AddItem", "flashlight", 1)
				removeItem()
		"Keys":
			if name == "Item_Keychain" :
				get_tree().call_group("Player", "AddKeys")
				get_tree().call_group("Level", "EndGame")
				removeItem()

	print("ending dialog")
	get_tree().call_group("Movable", "OnExaminablePlayerInteractingComplete")
	emit_signal("player_interacting_complete")
	print("player should be able to move")
	yield(get_tree().create_timer(0.2), "timeout")
	get_tree().paused = false
	canInteract = true
			
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

func _on_Area2D2_body_exited(body):
	print("exited area")
	if body.get_name() == "Player": 
		canInteract = false
