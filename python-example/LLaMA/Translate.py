from transformers import AutoTokenizer, AutoModelForCausalLM, pipeline

model_id = "meta-llama/Meta-Llama-3-8B-Instruct"
tokenizer = AutoTokenizer.from_pretrained(model_id)

model = AutoModelForCausalLM.from_pretrained(
    model_id,
    load_in_4bit=True,        # quantized loading
    device_map="auto",        # offload to available devices
)

pipe = pipeline("text-generation", model=model, tokenizer=tokenizer)
response = pipe("Translate Japanese to Chinese: 日本では、春になると桜が咲きます。", max_new_tokens=100)
print(response[0]["generated_text"])