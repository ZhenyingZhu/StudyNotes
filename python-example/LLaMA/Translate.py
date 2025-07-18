from transformers import AutoTokenizer, AutoModelForCausalLM, BitsAndBytesConfig, pipeline

model_id = "meta-llama/Meta-Llama-3-8B-Instruct"

bnb_config = BitsAndBytesConfig(
    load_in_4bit=True,
    bnb_4bit_quant_type="nf4",  # CPU-safe quantization
    bnb_4bit_use_double_quant=True,
)

tokenizer = AutoTokenizer.from_pretrained(model_id, use_auth_token=True)
model = AutoModelForCausalLM.from_pretrained(
    model_id,
    quantization_config=bnb_config,
    device_map="auto",
    use_auth_token=True,
)

pipe = pipeline("text-generation", model=model, tokenizer=tokenizer)
response = pipe("Translate Japanese to Chinese: 日本では、春になると桜が咲きます。", max_new_tokens=100)
print(response[0]["generated_text"])