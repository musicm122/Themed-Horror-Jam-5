extends "res://Scripts/GDScript/Examinable.gd"

# signal player_interacting
# signal player_interacting_complete

#export var timeline = ""
export var imagePath = ""

var imageSprite = null

# var interactAction = "interact"
# var canInteract = false

func IsInteracting():
	return Input.is_action_just_pressed(interactAction)

func onInteract():	
	canInteract = false
	var dialog = Dialogic.start(timeline) 
	emit_signal("player_interacting")
	dialog.connect("dialogic_signal", self, "dialog_listener")
	add_child(dialog)

func removeItem():
	get_tree().paused = false
	visible = false
	set_process(false)

func dialog_listener(arg):
	get_tree().paused = true
	print("dialog_listener called with", arg)

	match arg:
		"PizzaGiven":
			get_tree().call_group("Player", "RemoveItem", "pizza")
		"Pizza":
			get_tree().call_group("Player", "AddItem", "pizza", 1)
			if name == "Item_Pizza" :
				removeItem()
		"Flashlight":
			if name == "Item_Flashlight" :
				get_tree().call_group("Player", "AddFlashlight")
				removeItem()
		"Keys":
			if name == "Item_Keychain" :
				get_tree().call_group("Player", "AddKeys")
				get_tree().call_group("Level", "EndGame")
				removeItem()

		"start_dialog":
			print("on_interact_complete called")
			emit_signal("player_interacting")
			
		"end_dialog":
			print("on_interact_complete called")
			emit_signal("player_interacting_complete")
			yield(get_tree().create_timer(1.0), "timeout")
			get_tree().paused = false
			canInteract = true
			
# Called when the node enters the scene tree for the first time.
func _ready():
	if imagePath != "" :
		imageSprite = get_node_or_null(imagePath)
	


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
