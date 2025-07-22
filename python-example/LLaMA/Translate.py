from transformers import AutoTokenizer, AutoModelForCausalLM, pipeline, BitsAndBytesConfig

model_id = "meta-llama/Meta-Llama-3-8B-Instruct"

bnb_config = BitsAndBytesConfig(
    load_in_4bit=True, # quantized loading
    bnb_4bit_quant_type="fp4",  # for GPU
    bnb_4bit_compute_dtype="float16",
    bnb_4bit_use_double_quant=True
)

tokenizer = AutoTokenizer.from_pretrained(model_id, use_auth_token=True)

model = AutoModelForCausalLM.from_pretrained(
    model_id,
    quantization_config=bnb_config,
    device_map="auto",        # offload to available devices
    use_auth_token=True
)

input_text = "Translate Japanese to Chinese: 日本では、春になると桜が咲きます。"
inputs = tokenizer(input_text, return_tensors="pt").to("cuda")
output_ids = model.generate(**inputs, max_new_tokens=100)

for i in range(len(output_ids)):
    print(tokenizer.decode(output_ids[i], skip_special_tokens=True))
