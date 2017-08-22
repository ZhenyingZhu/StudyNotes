;; The first three lines of this file were inserted by DrRacket. They record metadata
;; about the language level of this file in a form that our tools can easily process.
#reader(lib "htdp-beginner-reader.ss" "lang")((modname image-larger) (read-case-sensitive #t) (teachpacks ()) (htdp-settings #(#t constructor repeating-decimal #f #t none #f () #f)))
(require 2htdp/image)

;; Image Image -> Boolean
;; Return true if the first image is larger than the second image by aera

(check-expect (area-larger? (rectangle 10 10 "solid" "black") (rectangle 15 15 "solid" "black")) false)
(check-expect (area-larger? (rectangle 10 15 "solid" "black") (rectangle 15 15 "solid" "black")) false)
(check-expect (area-larger? (rectangle 10 20 "solid" "black") (rectangle 15 15 "solid" "black")) false)
(check-expect (area-larger? (rectangle 15 10 "solid" "black") (rectangle 15 15 "solid" "black")) false)
(check-expect (area-larger? (rectangle 15 15 "solid" "black") (rectangle 15 15 "solid" "black")) false)
(check-expect (area-larger? (rectangle 15 20 "solid" "black") (rectangle 15 15 "solid" "black")) true)
(check-expect (area-larger? (rectangle 20 10 "solid" "black") (rectangle 15 15 "solid" "black")) false)
(check-expect (area-larger? (rectangle 20 15 "solid" "black") (rectangle 15 15 "solid" "black")) true)
(check-expect (area-larger? (rectangle 20 20 "solid" "black") (rectangle 15 15 "solid" "black")) true)

; (define (area-larger img1 img2) false) ; stub
; (define (area-larger img1 img2)        ; template
;    ... img1 img2)

(define (area-larger? img1 img2)
  (> (* (image-width img1) (image-height img1))
     (* (image-width img2) (image-height img2)) ) )